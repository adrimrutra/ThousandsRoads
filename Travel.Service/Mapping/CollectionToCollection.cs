using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel.Service.Mapping
{
	public class CollectionToCollection<TSource,TDest>
	{
		public List<TDest> Convert(ICollection<TSource> sources, object state)
		{
			if (sources == null)
				return new List<TDest>();
			var mapper = new EmitObjectMapper();
			var dests = new List<TDest>();
			foreach (var source in sources)
			{
				dests.Add(mapper.Map<TSource, TDest>(source));
			}
			
			return dests;
		}
	}
}
