using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Core.Data;
using Travel.Service.Mapping;
using Travel.Service.Services.Abstractions;
using System.Data.Entity;
using Travel.Core;
using Travel.Entity;
using Travel.Common.Enums;
using NLog;
using Autofac;

namespace Travel.Service.Services.Implementation
{

	public class UserService : IUserService
	{

		#region Dependency
		private IDbSession _dbSession { set; get; }
		private IObjectMapper _mapper { set; get; }
		protected static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		public UserService(IDbSession dbSession, IObjectMapper mapper)
		{
			this._dbSession = dbSession;
			this._mapper = mapper;
		}
		#endregion Dependency

		public Data.User Get(string SocialId, Common.Enums.TokenType tokenType)
		{
			return
				_mapper.Map<Entity.User, Data.User>(
				_dbSession
				.AsQueryable<Entity.User>()
				.Include(x => x.Tokens)
				.SingleOrDefault(x => x.Tokens.Any(m => m.SocialId == SocialId && m.Tokentype == tokenType))
				);
		}

		#region FriendArea

		public void UpdateFriedsList(Authentication.IAuthentification auth)
		{

			//var database = (IDbSession)((Autofac.Core.Container)ApplicationContainer.Container).GetService(typeof(IDbSession));
			//var database = (IDbSession)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IDbSession));
			using (var scope = ApplicationContainer.Container.BeginLifetimeScope())
			{
				//var database = (IDbSession)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IDbSession));
				var database = scope.Resolve<IDbSession>();




				using (var trance = database.BeginTransaction())
				{
					Entity.User user = null;
					#region GetFriendListItem
					try
					{
						user = database.AsQueryable<Entity.User>()
						.Include(x => x.FriendListItems)
							//.Include(x => x.FriendListItems.Select(m => m.CustomerList))
							//.Include(x => x.FriendListItems.Select(m => m.Users))
							//.Include(x => x.FriendListItems.Select(m => m.Owner))
						.SingleOrDefault(x => x.Id == auth.ThUser.Id);
					}
					catch (Exception ex)
					{
						_logger.ErrorException("User Service line 74\t\n", ex);
						//_logger.Info<Data.User>(auth.ThUser);
					}
					//var friendListItem = user.FriendListItems.SingleOrDefault(x => x.OwnerId == user.Id);
					user.FriendListItems.Clear();
					var friendListItem = new FriendListItem
						{
							Owner = user
						};
					user.FriendListItems.Add(friendListItem);

					#endregion GetFriendListItem
					if (friendListItem.CustomerList == null)
						friendListItem.CustomerList = new FriendsList();
					if (friendListItem.CustomerList.Users == null)
						friendListItem.CustomerList.Users = new List<User>();

					friendListItem.CustomerList.Users.Clear();
					var friendList = auth.User.GetFriends();
					foreach (var friend in friendList)
					{
						var frToken = friend.Id.ToString();
						Entity.User frUser = null;
						try
						{

							frUser = database.AsQueryable<Entity.User>()
									.Include(x => x.FriendListItems)
									//.Include(x => x.FriendListItems.Select(m => m.CustomerList))
									//.Include(x => x.FriendListItems.Select(m => m.Users))
									//.Include(x => x.FriendListItems.Select(m => m.Owner))
									.SingleOrDefault(x => x.Tokens.Any(m => m.SocialId == frToken && m.Tokentype == auth.Type));
						}
						catch (Exception ex)
						{
							_logger.ErrorException("User Service line 107\t\n", ex);
							//_logger.Info<Data.User>(auth.ThUser);
						}
						if (frUser != null)
						{
							var frlist = frUser.FriendListItems.SingleOrDefault(x => x.OwnerId == frUser.Id);
							if (frlist != null)
							{
								user.FriendListItems.Add(frlist);
								if (!frUser.FriendListItems.Any(x => x.Id == friendListItem.Id))
									frUser.FriendListItems.Add(friendListItem);
								friendListItem.CustomerList.Users.Add(frUser);
								//database.Save(frUser);
							}
						}
					}
					#region Seva List
					database.Save(user);
					trance.Commit();
					#endregion Seva List
				}
			}

		}

		public IEnumerable<Data.MyFriend> GetFriends(int userId, int driverId)
		{

			var driver = _dbSession.AsQueryable<Entity.User>()
				.Include(x => x.FriendListItems)
				.Include(x => x.FriendListItems.Select(m => m.Owner))
				.SingleOrDefault(x => x.Id == driverId);

			if (driver == null)
				return new List<Data.MyFriend>();

			if (userId == driverId)
				return new List<Data.MyFriend> { 
				 new Data.MyFriend
					{
						Id = driver.Id,
						Name = driver.DisplayName,
						Avatar = driver.Avatar,
						Type = UserType.Driver
					}
				};

			var user = _dbSession.AsQueryable<Entity.User>()
				.Include(x => x.FriendListItems)
				.Include(x => x.FriendListItems.Select(m => m.Owner))
				.SingleOrDefault(x => x.Id == userId);

			if (user == null)
			{
				return new List<Data.MyFriend> { 
				 new Data.MyFriend
					{
						Id = driver.Id,
						Name = driver.DisplayName,
						Avatar = driver.Avatar,
						Type = UserType.Driver
					}
				};
			}

			var friends = user.FriendListItems.Intersect(
				driver.FriendListItems,
				AnonymousComparer.Create<Entity.FriendListItem>((c1, c2) => c1.Id == c2.Id,
				c1 => c1.Id.GetHashCode())).ToList();
			if (friends.Count > 0)
			{
				var friend = friends.Select(x => x.Owner).FirstOrDefault();
				var list = new List<Entity.User>();
				list.Add(friend);
				list.Insert(0, driver);
				list.Add(user);
				var res = list.Distinct(
					 AnonymousComparer.Create<Entity.User>((c1, c2) => c1.Id == c2.Id,
						 c1 => c1.Id.GetHashCode()))
						 .ToList()
						 .Select(x =>
						 {
							 UserType type = UserType.Passenger;
							 if (x.Id == driver.Id)
								 type = UserType.Driver;
							 Data.MyFriend item = new Data.MyFriend
							 {
								 Id = x.Id,
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
						Id = driver.Id,
						Name = driver.DisplayName,
						Avatar = driver.Avatar,
						Type = UserType.Driver
					}
				};
		}

		#endregion FriendArea

		public IEnumerable<Data.User> GetAll()
		{
			throw new NotImplementedException();
		}

		public Data.User Get(int id)
		{
			return _mapper.Map<Entity.User, Data.User>(
					_dbSession.AsQueryable<Entity.User>()
					.SingleOrDefault(x => x.Id == id)
				);
		}

		public Data.User Add(Data.User model)
		{
			using (var trance = _dbSession.BeginTransaction())
			{
				var user = new Entity.User
				{
					DisplayName = model.DisplayName,
					Avatar = model.Avatar,
					Email = model.Email,
					Tokens = model.Tokens.Select(x => new Entity.Token
					{
						Tokentype = x.Tokentype.Value,
						SocialId = x.SocialId
					}).ToList(),
				};
				_dbSession.Insert<Entity.User>(user);
				trance.Commit();
				return _mapper.Map<Entity.User, Data.User>(user);
			}
		}

		public void Delete(int id)
		{
			using (var trance = _dbSession.BeginTransaction())
			{
				_dbSession.Delete<Entity.User>(x => x.Id == id);
				trance.Commit();
			}
		}

		public Data.User Save(Data.User model)
		{
			using (var trance = _dbSession.BeginTransaction())
			{
				var user = _dbSession.AsQueryable<Entity.User>().SingleOrDefault(x => x.Id == model.Id);
				if (user == null)
					return null;
				user.DisplayName = model.DisplayName;
				user.Avatar = model.Avatar;
				user.Rating = model.Rating;
				user.Tokens.Clear();
				foreach (var token in model.Tokens)
				{
					var dbToken = _dbSession.AsQueryable<Entity.Token>()
						.SingleOrDefault(x => x.SocialId == token.SocialId);
					if (dbToken == null)
					{
						dbToken = new Entity.Token
						{
							SocialId = token.SocialId,
							Tokentype = token.Tokentype.Value
						};
					}

					user.Tokens.Add(dbToken);
				}
				_dbSession.Save<Entity.User>(user);
				return _mapper.Map<Entity.User, Data.User>(user);
			}
		}
	}
}
