using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Travel.Web.Api.Authorization;

namespace Travel.Web.Api.Controllers.Base
{
	public abstract class ApiControllerBase<T>: ApiController
	{
		//must be comments
		//public abstract IEnumerable<T> GetAll();
		public abstract T Get(int Id);
		public abstract void Delete(int Id);
		public abstract T Post(T _object);
		public abstract T Put(int Id, T _object);
	}
}
