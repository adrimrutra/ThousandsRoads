using InterSystems.Data.CacheTypes;
using System.Collections.Generic;
using System.Reflection;
using Travel.Core;
using System;

namespace Travel.CacheMigration
{
	public class CacheConvertCollectionExtends<TDest> where TDest : class,new()
	{
		private IObjectMapper mapper;
		public CacheConvertCollectionExtends()
		{
			mapper = (IObjectMapper)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IObjectMapper));
		}
		public List<TDest> Convert(CacheListOfObjects source, object state)
		{
			var result = new List<TDest>();
			if (source.Count > 0)
			{
				foreach (var point in source)
				{
					if (point != null)
					{
						var type = point.GetType();
						var _mapper = mapper.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)[0];
						_mapper = _mapper.MakeGenericMethod(new Type[] { type, typeof(TDest) });
						result.Add((TDest)_mapper.Invoke(mapper, new object[] { point }));
					}
				}
			}
			return result;
		}
	}
}
