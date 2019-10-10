using System.Collections.Generic;

namespace Travel.Service.Services
{
	public interface IService<T> where T : class
	{
		IEnumerable<T> GetAll();
		T Get(int id);
		T Add(T model);
		void Delete(int id);
		T Save(T model);
	}
}
