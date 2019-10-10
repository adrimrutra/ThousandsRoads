using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Service.Services.Abstractions;

namespace Travel.Service.Search
{
	public interface ISearchIndexService
	{
		void RebuildIndex();
	}

	public class SearchIndexService : ISearchIndexService
	{
		private readonly ISearchProvider _provider;
		private readonly ITravelService _service;

		public SearchIndexService(ISearchProvider provider, ITravelService service)
		{
			this._provider = provider;
			this._service = service;
		}

		public void RebuildIndex()
		{
			const bool createOrOwerwriteIndex = true;

			if (!IndexWriter.IsLocked(_provider.IndexDirectory))
				using (IndexWriter iw = new IndexWriter(_provider.IndexDirectory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), createOrOwerwriteIndex, IndexWriter.MaxFieldLength.UNLIMITED))
				{
					var travels = _service.GetAll();
					foreach (var travel in travels)
					{
						iw.AddDocument(SearchTravelService.ToDocument(travel));
					}

					iw.Optimize();
				}
		}
	}
}
