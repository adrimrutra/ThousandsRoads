using System;
using System.Collections.Generic;
using Travel.Service.Services.Abstractions;

using Travel.CasheMigration;
using Travel.CasheService.Services.Abstraction;
using Travel.Core.Exceptions;
using InterSystems.Data.CacheTypes;


namespace Travel.CasheService.Services.Implementation
{
	public class CommentCacheService : BaseCacheService<ThousandsRoads.Comment, Data.Comment>, ICommentService
	{
		public CommentCacheService(CacheSession<ThousandsRoads.Comment, Data.Comment> session) : base(session) { }

		public override Data.Comment Add(Data.Comment model)
		{
			using (var session = this.cacheSession.Open())
			{
				var user = ThousandsRoads.Customer.OpenId(session, model.UserId.ToString());
				var messenger = ThousandsRoads.Customer.OpenId(session, model.MessengerId.ToString());
				if (user == null || Int32.Parse(user.Id()) == 0)
				{
					throw new SomethingWrongException(new ArgumentNullException("added comment to nulleble user"));
				}
				if (messenger == null || Int32.Parse(messenger.Id()) == 0)
				{
					throw new NotAuthorizedException();
				}
				var comment = new ThousandsRoads.Comment(session);
				comment.Messenger = messenger;
				comment.User = user;
				comment.UserId = Int64.Parse(user.Id());
				comment.MessengerId = model.MessengerId;
				comment.Message = model.Message;
				comment.Type = (long)model.Type;
				comment.Data = DateTime.UtcNow;
				//comment.Save();
				user.Comments.Add(comment);
				user.Save();
				return this.cacheSession.Mapper.Map<ThousandsRoads.Comment, Data.Comment>(comment);
			}
		}

		public IEnumerable<Data.Comment> GetAll(int userId)
		{
			using (var session = this.cacheSession.Open())
			{
				var user = ThousandsRoads.Customer.OpenId(session, userId.ToString());
				var dataUser = this.cacheSession.Mapper.Map<ThousandsRoads.Customer, Data.User>(user);
				return dataUser.Comments;
			}
		}

		public Data.Comment Get(int id, int userId)
		{
			using (var session = this.cacheSession.Open())
			{
				var comment = ThousandsRoads.Comment.OpenId(session, id.ToString());
				if(comment.UserId == userId)
					return this.cacheSession.Mapper.Map<ThousandsRoads.Comment, Data.Comment>(comment);
				return new Data.Comment();
			}
		}

		public override Data.Comment Save(Data.Comment model)
		{
			throw new NotImplementedException();
		}
	}
}