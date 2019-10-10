using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Service.Services.Abstractions;
using InterSystems.Data.CacheClient;
using InterSystems.Data.CacheTypes;
using ThousandsRoads;
using Travel.CasheMigration;
using Data = Travel.Data;
using System.Data;
using Travel.Core;

namespace Travel.CasheService.Services.Abstraction
{
	public abstract class BaseCacheService<Tsource, Tdest>
		where Tsource : CacheObject
		where Tdest : class
	{

		#region Dependency
		protected CacheSession<Tsource, Tdest> cacheSession;
		public BaseCacheService(CacheSession<Tsource, Tdest> session)
		{
			this.cacheSession = session;
		}
		#endregion Dependency

		public virtual IEnumerable<Tdest> GetAll()
		{
			using (var session = this.cacheSession.Open())
			{
				this.cacheSession.SelectCommand.CommandText = "SELECT * FROM " + typeof(Tsource).FullName;
				return this.cacheSession.GetAll();//.Map<Tsource, Tdest>(mapper.Map<Tsource, Tdest>);
			}
		}
		public virtual Tdest Get(int id)
		{
			using (var session = this.cacheSession.Open())
			{
				//var result = mapper.Map<Tsource, Tdest>(this.cacheSession.Get(id));
				//return result;
				return this.cacheSession.Get(id);
			}
		}
		public virtual void Delete(int id)
		{
			using (var session = this.cacheSession.Open())
			{
				this.cacheSession.Delete(id);
			}
		}
		public abstract Tdest Add(Tdest model);
		public abstract Tdest Save(Tdest model);

	}
}
