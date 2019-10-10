using InterSystems.Data.CacheClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Authentication;
using Travel.CasheMigration;
using Travel.CasheService.Services.Abstraction;
using Travel.Service.Services.Abstractions;

namespace Travel.CasheService.Services.Implementation
{
	public class MessageCacheService : BaseCacheService<ThousandsRoads.Message, Data.Message>, IMessageService
	{
		public MessageCacheService(CacheSession<ThousandsRoads.Message, Data.Message> session)
			: base(session)
		{
		}

		private readonly string selectUserMessages = @"select 
														ThousandsRoads.Message.ID, Direction, Luggage, MessageText, 
														ThousandsRoads.Customer.Email as UserEmail,
														MessengerId, PersonCount, State,Type, Theme, TravelId, UserId,
														Avatar as MessengerAvatar, DisplayName as MessengerDisplayName, Email as MessengerEmail
														from ThousandsRoads.Message
														left join ThousandsRoads.Customer
														on MessengerId = ThousandsRoads.Customer.ID
														where UserId = {0}";

		public IEnumerable<Data.Message> GetAll(int user)
		{
			var userId = user.ToString();
			using (var session = this.cacheSession.Open())
			{
				var com = string.Format(selectUserMessages, userId);
				this.cacheSession.SelectCommand.CommandText = com;
				return this.cacheSession.GetAll();
			}

		}

		public Data.Message Get(int id, int userId)
		{
			using (var session = this.cacheSession.Open())
			{
				var message = ThousandsRoads.Message.OpenId(session, id.ToString());
				if (message.UserId == userId)
					return this.cacheSession.Mapper.Map<ThousandsRoads.Message, Data.Message>( message);
				return new Data.Message();
			}
		}
		public override Data.Message Add(Data.Message model)
		{
			using (var session = this.cacheSession.Open())
			{
				var message = new ThousandsRoads.Message(session);
				SaveStrategy(model, message);
				var result = this.cacheSession.Add(message);
				return result;
			}
		}
		private void SaveStrategy(Data.Message model, ThousandsRoads.Message message)
		{
			message.UserId = model.UserId;
			message.MessengerId = model.MessengerId;
			message.TravelId = model.TravelId;
			message.State = model.State;
			message.Type = (long)(int)model.Type;
			message.Theme = model.Theme;
			message.MessageText = model.MessageText;
			message.Direction = (long)model.Direction;
			message.Luggage = (long)model.Luggage;
			message.PersonCount = model.PersonCount;
		}

		public override Data.Message Save(Data.Message message)
		{
			using (var session = this.cacheSession.Open())
			{
				var entity = ThousandsRoads.Message.OpenId(session, message.Id.ToString());
				entity.State = message.State;
				entity.Type = (long)message.Type;
				entity.Save();
				return this.cacheSession.Mapper.Map<ThousandsRoads.Message, Data.Message>(entity);
			}
			throw new ArgumentException("something wrong");
		}
	}
}
