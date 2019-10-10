using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Data;

namespace Travel.Service.Services.Abstractions
{
	public interface ICommentService : IService<Comment>
	{
		IEnumerable<Data.Comment> GetAll(int userId);
		Data.Comment Get(int id, int userId);
	}
}
