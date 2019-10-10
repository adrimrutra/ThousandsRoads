using System.Collections.Generic;
using Travel.Core;
using Travel.Core.Data;
using Travel.Service.Services.Abstractions;
using System.Linq;
using Travel.Authentication;
using Travel.Service.Mapping;
using System.Data.Entity;
namespace Travel.Service.Services.Implementation
{
	public class MessageService : IMessageService
	{

		#region Dependency
		private IDbSession _dbSession { set; get; }
		private IObjectMapper _mapper { set; get; }
		public MessageService(IDbSession dbSession, IObjectMapper mapper)
		{
			_dbSession = dbSession;
			_mapper = mapper;
		}
		#endregion Dependency

		public Data.Message Get(int id, int userid)
		{
			return _mapper.Map<Entity.Message, Data.Message>(
				_dbSession.AsQueryable<Entity.Message>()
				.Include(x => x.User)
				.Include(x => x.Messenger)
				.Include(x => x.Travel)
				.SingleOrDefault(x => x.Id == id && x.UserId == userid)
				);
		}

		public Data.Message Add(Data.Message model)
		{
			using (var trance = _dbSession.BeginTransaction())
			{
				var entity = new Entity.Message
				{
					Luggage = model.Luggage,
					MessageText = model.MessageText,
					MessengerId = model.MessengerId,
					PersonCount = model.PersonCount,
					State = model.State,
					Theme = model.Theme,
					TravelId = model.TravelId,
					UserId = model.UserId,
					Type = model.Type
				};
				_dbSession.Insert(entity);
				trance.Commit();
				return _mapper.Map<Entity.Message, Data.Message>(entity);
			}
		}

		public void Delete(int id)
		{
			using (var trance = _dbSession.BeginTransaction())
			{
				var entity = _dbSession.AsQueryable<Entity.Message>()
					.SingleOrDefault(x => x.Id == id);
				_dbSession.Delete(entity);
				trance.Commit();
			}
		}

		public Data.Message Save(Data.Message model)
		{
			using (var trance = _dbSession.BeginTransaction())
			{
				var entity = _dbSession.AsQueryable<Entity.Message>()
					.SingleOrDefault(x => x.Id == model.Id);
				if (entity != null)
				{
					entity.State = model.State;
					entity.Type = model.Type;
					_dbSession.Save(entity);
					trance.Commit();
				}
				return _mapper.Map<Entity.Message, Data.Message>(entity);
			}
		}

		public IEnumerable<Data.Message> GetAll(int userid)
		{
			return _dbSession.AsQueryable<Entity.Message>()
				.Include(x => x.User)
				.Include(x => x.Messenger)
				.Include(x => x.Travel)
				.Where(x => x.UserId == userid)
				.Map<Entity.Message, Data.Message>(_mapper.Map<Entity.Message, Data.Message>);
		}

		public IEnumerable<Data.Message> GetAll()
		{
			throw new System.NotImplementedException();
		}

		public Data.Message Get(int id)
		{
			throw new System.NotImplementedException();
		}
	}
}
