using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Travel.Service.Mapping
{
	public static class Extension
	{
		public static IEnumerable<TDest> Map<TSource, TDest>(this IEnumerable<TSource> Source, Func<TSource, TDest> Mapper)
		{
			return Source.Select(Mapper); 
		}
		public static IQueryable<TDest> Map<TSource, TDest>(this IQueryable<TSource> Source, Expression <Func<TSource, TDest>> Mapper)
		{
			return Source.Select(Mapper);
		}
	}
}
