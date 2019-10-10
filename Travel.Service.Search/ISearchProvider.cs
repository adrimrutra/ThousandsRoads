using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel.Service.Search
{
	public interface ISearchProvider
	{
		Lucene.Net.Store.Directory IndexDirectory { get; }
		bool NeedsIndexRebuildAtStartup { get; }
		DateTimeOffset LastModified
		{
			get;
		}
	}
}
