using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Core.Data;
using Travel.Service.Services.Abstractions;
using Travel.Service.Mapping;
using System.Data.Entity;
using Travel.Core;

namespace Travel.Service.Services.Implementation
{
	public class CommentService : ICommentService
	{
		#region Dependecy
		public IDbSession _dbSession { set; get; }
		public IObjectMapper _mapper { set; get; }
		public CommentService(IDbSession dbSession, IObjectMapper mapper)
		{
			this._dbSession = dbSession;
			this._mapper = mapper;
		}
		#endregion Dependecy

		public Data.Comment Add(Data.Comment model)
		{
			using (var transaction = _dbSession.BeginTransaction())
			{
				var user = _dbSession.AsQueryable<Entity.User>()
					.SingleOrDefault(x => x.Id == model.MessengerId);
				var comment = new Entity.Comment
				{
					Data = DateTimeOffset.UtcNow,
					Message = model.Message,
					MessengerId = user.Id,
					Messenger = user,
					UserId = model.UserId,
					Type = model.Type
				};
				_dbSession.Insert(comment);
				transaction.Commit();
				return _mapper.Map<Travel.Entity.Comment, Data.Comment>(comment);
			}

		}

		public void Delete(int id)
		{
			using (var transaction = _dbSession.BeginTransaction())
			{
				_dbSession.Delete<Travel.Entity.Comment>(x => x.Id == id);
				transaction.Commit();
			}
		}

		public Data.Comment Save(Data.Comment model)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Data.Comment> GetAll(int userId)
		{
			return _dbSession.AsQueryable<Travel.Entity.Comment>()
				.Include(x => x.User)
				.Include(x=>x.Messenger)
				.Where(x=>x.UserId == userId)
				.Map<Travel.Entity.Comment, Data.Comment>(_mapper.Map<Travel.Entity.Comment, Data.Comment>);
		}

		public Data.Comment Get(int id, int userId)
		{
			var entity = _dbSession.AsQueryable<Travel.Entity.Comment>()
				.Include(x => x.User)
				.SingleOrDefault(x => x.Id == id && x.UserId == userId);
			return _mapper.Map<Travel.Entity.Comment, Data.Comment>(entity);
		}

		public IEnumerable<Data.Comment> GetAll()
		{
			throw new NotImplementedException();
		}

		public Data.Comment Get(int id)
		{
			throw new NotImplementedException();
		}
	}
}