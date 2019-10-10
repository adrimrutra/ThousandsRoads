using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Core.Data;

namespace Travel.Migration
{
	public class DbSession : EFSession
	{
		public DbSession(DbContext aDataContext)
			: base(aDataContext) { }

		public IQueryable<T> Secure<T>() where T : class
		{
			return base.DataContext.CreateObjectSet<T>();
		}
	}
}
