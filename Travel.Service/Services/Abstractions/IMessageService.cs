using System.Collections.Generic;
using Travel.Data;

namespace Travel.Service.Services.Abstractions
{
	public interface IMessageService : IService<Message>
	{
		Data.Message Get(int id, int userid);
		IEnumerable<Message> GetAll(int userid);
	}
}
