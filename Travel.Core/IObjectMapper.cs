using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel.Core
{
	public interface IObjectMapper
	{
		TDest Map<TSource, TDest>(TSource Object);
		TDest Map<TSource, TDest>(TSource Object, Func<TSource, TDest> Mapper);
		Func<TSource, TDest> GetMapper<TSource, TDest>();
	}
}
