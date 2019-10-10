using System.Linq;
using Travel.Service.Services.Abstractions;

using InterSystems.Data.CacheTypes;
using InterSystems.Data.CacheClient;
using ThousandsRoads;
using Travel.CasheMigration;
using Travel.CasheService.Services.Abstraction;
using Travel.Common.Enums;
using Travel.Authentication;
using System.Collections.Generic;
using System.Collections;
using Travel.Core;

namespace Travel.CasheService.Services.Implementation
{
	public class UserCacheService : BaseCacheService<ThousandsRoads.Customer, Data.User>, IUserService
	{

		public UserCacheService(CacheSession<ThousandsRoads.Customer, Data.User> session)
			: base(session) { }
		private readonly string selectSocialId = "select ID from ThousandsRoads.Token where SocialId = {0} and Tokentype = {1}";

		public override Data.User Save(Data.User model)
		{
			using (var session = this.cacheSession.Open())
			{
				var customer = ThousandsRoads.Customer.OpenId(session, model.Id.ToString());
				SaveStrategy(model, customer);
				return this.cacheSession.Mapper.Map<ThousandsRoads.Customer, Data.User>(customer);
			}
		}

		public override Data.User Add(Data.User model)
		{
			using (var session = this.cacheSession.Open())
			{
				var customer = new ThousandsRoads.Customer(session);
				SaveStrategy(model, customer);
				var result = this.cacheSession.Add(customer);
				return result;
			}
		}

		private void SaveStrategy(Data.User model, ThousandsRoads.Customer customer)
		{
			customer.DisplayName = model.DisplayName;
			customer.Avatar = model.Avatar;
			customer.Email = model.Email;
			foreach (var token in model.Tokens)
			{
				var tmp = new Token(customer.Connection);
				tmp.SocialId = token.SocialId;
				tmp.Tokentype = (long)(int)token.Tokentype;
				tmp.User = customer;
				customer.Tokens.Add(tmp);
			}
		}

		public Data.User Get(string SocialId, TokenType tokentype)
		{
			using (var session = this.cacheSession.Open())
			{
				ThousandsRoads.Token token = null;
				var com = string.Format(selectSocialId, SocialId, (int)tokentype);
				var command = new CacheCommand(com);
				command.Connection = session;
				var tokenId = this.cacheSession.GetScalar(command);
				if (tokenId != null)
					token = ThousandsRoads.Token.OpenId(session, tokenId.ToString());
				if (token == null || token.User == null)
					return new Data.User();
				return this.cacheSession.Mapper.Map<ThousandsRoads.Customer, Data.User>(token.User);
			}
		}

		public void UpdateFriedsList(IAuthentification auth)
		{

			ApplicationContainer.Container.BeginLifetimeScope();

			//get all oauth friends
			var friendList = auth.User.GetFriends();

			using (var session = this.cacheSession.Open())
			{
				#region Find user
				var customer = ThousandsRoads.Customer.OpenId(session, auth.ThUser.Id.ToString());
				if (customer == null)
					return;
				#endregion Find user
				#region find my owner list
				if (customer.FriendListItems == null)
					customer.FriendListItems = new CacheListOfObjects();

				//((IList<FriendListItem>)customer.FriendListItems).Select(x => x.Id());

				ThousandsRoads.FriendListItem friendListItem = null;
				//find owner`s friend list 
				foreach (ThousandsRoads.FriendListItem list in customer.FriendListItems)
				{
					if (list.Owner.Id() == customer.Id())
					{
						friendListItem = list;

						break;
					}
				}
				//if list not found = create

				if (friendListItem == null)
				{
					friendListItem = new ThousandsRoads.FriendListItem(customer.Connection);
					friendListItem.Owner = customer;
				}
				if(!((IList)customer.FriendListItems).Cast<ThousandsRoads.FriendListItem>()
					.Any(x=>x.Id() == friendListItem.Id()))
					customer.FriendListItems.Add(friendListItem);

				#endregion find my owner list
				#region customrs list
				//create owner`s customer list
				if (friendListItem.CustomerList == null)
					friendListItem.CustomerList = new ThousandsRoads.FriendList(customer.Connection);

				friendListItem.CustomerList.ParentList = friendListItem;

				if (friendListItem.CustomerList.Customers == null)
					friendListItem.CustomerList.Customers = new CacheListOfObjects();
				else
					friendListItem.CustomerList.Customers.Clear();

				#endregion customrs list
				#region Find friends
				foreach (var friend in friendList)
				{
					//find friend by token
					#region Find friend
					ThousandsRoads.Token token = null;

					var com = string.Format(selectSocialId, friend.Id, (int)auth.Type);
					var command = new CacheCommand(com);
					command.Connection = session;

					var tokenId = this.cacheSession.GetScalar(command);

					if (tokenId != null)
						token = ThousandsRoads.Token.OpenId(session, tokenId.ToString());

					#endregion  Find friend
					if (token != null && token.User != null)
					{
						//add friend to my list
						friendListItem.CustomerList.Customers.Add(token.User);

						var list = token.User.FriendListItems.Cast<FriendListItem>()
							.SingleOrDefault(x => x.Owner.Id() == token.User.Id());
						if (list != null)
						{
							list.CustomerList.Customers.Add(customer);
							token.User.FriendListItems.Add(friendListItem);
							if (!((IList)customer.FriendListItems).Cast<ThousandsRoads.FriendListItem>()
								.Any(x => x.Id() == list.Id()))
							customer.FriendListItems.Add(list);
							token.User.Save();
						}

					}
				}
				#endregion Find friends
				customer.Save();
			}

		}

		public IEnumerable<Data.MyFriend> GetFriends(int userId, int driverId)
		{
			using (var session = this.cacheSession.Open())
			{
				var user = ThousandsRoads.Customer.OpenId(session, userId.ToString());
				var userList = ((IList)user.FriendListItems)
					.Cast<ThousandsRoads.FriendListItem>()
					.Select(x => x);//new { Id = x.Id(), Owner = x.Owner }
				if (userId == driverId)
					return new List<Data.MyFriend> {
						new Data.MyFriend
							 {
								 Id = int.Parse(user.Id()),
								 Name = user.DisplayName,
								 Avatar = user.Avatar,
								 Type = UserType.Driver
							 }};

				var driver = ThousandsRoads.Customer.OpenId(session, driverId.ToString());
				if (driver == null || int.Parse(driver.Id()) == 0)
					return new List<Data.MyFriend>();
				var driverIdList = ((IList)driver.FriendListItems)
					.Cast<ThousandsRoads.FriendListItem>()
					.Select(x => x);//new { Id = x.Id(), Owner = x.Owner }

				var friends = userList.Intersect(driverIdList,
					 AnonymousComparer.Create<ThousandsRoads.FriendListItem>((c1, c2) => c1.Id() == c2.Id(),
					 c1 => c1.Id().GetHashCode())).ToList();
				if (friends.Count() > 0)
				{
					var friend = friends.Select(x => x.Owner).FirstOrDefault();
					List<ThousandsRoads.Customer> list = new List<Customer>();
					list.Add(friend);
					list.Insert(0, driver);
					list.Add(user);
					var res = list.Distinct(
						 AnonymousComparer.Create<ThousandsRoads.Customer>((c1, c2) => c1.Id() == c2.Id(),
						 c1 => c1.Id().GetHashCode()))
						 .ToList()
						 .Select(x =>
						 {
							 UserType type = UserType.Passenger;
							 if (x.Id() == driver.Id())
								 type = UserType.Driver;
							 Data.MyFriend item = new Data.MyFriend
							 {
								 Id = int.Parse(x.Id()),
								 Name = x.DisplayName,
								 Avatar = x.Avatar,
								 Type = type
							 };
							 return item;
						 }).ToList();
					return res;
				}
				return new List<Data.MyFriend> {
					new Data.MyFriend
					{
						Id = int.Parse(driver.Id()),
						Name = driver.DisplayName,
						Avatar = driver.Avatar,
						Type = UserType.Driver
					}};
			}
		}
	}
}
