using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Data;

namespace Travel.Service.Search
{
	public class SearchTravelService : ISearchTravelService
	{
		#region Const String Filds
		private const string IdField = "Id";
		private const string MapPoints = "MapPointsName";
		private const string MapPointsLat = "MapPointsLat";
		private const string MapPointsLon = "MapPointsLon";

		private const string EndDate = "EndDate";
		private const string StartDate = "StartDate";
		private const string Direction = "Direction";
		private const string Luggage = "Luggage";
		private const string Wait = "Wait";
		private const string Early = "Early";
		private const string Capacity = "Capacity";

		private const string DateFormate = "yyyyMMdd";
		#endregion Const String Filds

		private readonly ISearchProvider _provider;

		public SearchTravelService(ISearchProvider provider)
		{
			this._provider = provider;
		}

		public IEnumerable<int> Search(SearchTravel travelOption)
		{
			List<int> results = new List<int>();
			int maxDocs = 0;

			if (IndexReader.IndexExists(_provider.IndexDirectory))
				using (var searcher = new IndexSearcher(_provider.IndexDirectory))
				{
					try
					{
						BooleanQuery bq = new BooleanQuery();
						TopDocs hits = null;
						#region StartAddressName
						if (!string.IsNullOrEmpty(travelOption.Saddress))//start Point address
						{
							Analyzer analyzer = new WhitespaceAnalyzer();
							var queries = travelOption.Saddress.Replace(", ", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
							foreach (var query in queries)
							{
								QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, MapPoints, analyzer);
								if (query.Split(new char[] { ' ' }).Count() > 1)
								{
									Query qu = parser.Parse("\"" + query + "\"");
									bq.Add(qu, Occur.SHOULD);

								}
								else
								{
									Query qu = parser.Parse(query + "*");
									bq.Add(qu, Occur.SHOULD);
								}
							}
						}
						#endregion StartAddressName
						#region EndAddressName
						if (!string.IsNullOrEmpty(travelOption.Eaddress))
						{
							Analyzer analyzer = new WhitespaceAnalyzer();
							var queries = travelOption.Eaddress.Replace(", ", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
							foreach (var query in queries)
							{
								QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, MapPoints, analyzer);
								if (query.Split(new char[] { ' ' }).Count() > 1)
								{
									Query qu = parser.Parse("\"" + query + "\"");
									bq.Add(qu, Occur.SHOULD);

								}
								else
								{
									Query qu = parser.Parse(query + "*");
									bq.Add(qu, Occur.SHOULD);
								}
							}
						}
						#endregion EndAddressName
						#region MapPoints
						if (travelOption.Slat.HasValue && travelOption.Slon.HasValue
							&& !Double.IsNaN(travelOption.Slat.Value)
							&& !Double.IsNaN(travelOption.Slon.Value))
						{
							var queryParserLat = new TermRangeQuery(MapPointsLat, (travelOption.Slat - 0.003).ToString(),
								(travelOption.Slat + 0.003).ToString(), true, true);
							bq.Add(queryParserLat, Occur.SHOULD);
							var queryParserLon = new TermRangeQuery(MapPointsLon, (travelOption.Slon - 0.003).ToString(),
								(travelOption.Slon + 0.003).ToString(), true, true);
							bq.Add(queryParserLon, Occur.SHOULD);

						}
						if (travelOption.Elat.HasValue && travelOption.Elon.HasValue
							&& !Double.IsNaN(travelOption.Elat.Value)
							&& !Double.IsNaN(travelOption.Elon.Value))
						{
							var queryParserLat = new TermRangeQuery(MapPointsLat, (travelOption.Elat - 0.003).ToString(),
								(travelOption.Elat + 0.003).ToString(), true, true);
							bq.Add(queryParserLat, Occur.SHOULD);
							var queryParserLon = new TermRangeQuery(MapPointsLon, (travelOption.Elon - 0.003).ToString(),
								(travelOption.Elon + 0.003).ToString(), true, true);
							bq.Add(queryParserLon, Occur.SHOULD);
						}
						#endregion MapPoints
						#region Date

						if (travelOption.Sdate.HasValue)
							bq.Add(NumericRangeQuery.NewIntRange(StartDate,
										int.Parse(travelOption.Sdate.Value.ToString(DateFormate)),
										null,
										true, true), Occur.SHOULD);
						if (travelOption.Edate.HasValue)
							bq.Add(NumericRangeQuery.NewIntRange(EndDate,
									null,
									int.Parse(travelOption.Edate.Value.ToString(DateFormate)),
									true, true), Occur.SHOULD);

						#endregion Date
						#region Other
						if (travelOption.Capacity.HasValue)
						{
							Analyzer analyzer = new WhitespaceAnalyzer();
							QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, Capacity, analyzer);
							Query qu = parser.Parse(travelOption.Capacity.ToString());
							bq.Add(qu, Occur.SHOULD);
						}
						if (travelOption.Luggage.HasValue)
						{
							Analyzer analyzer = new WhitespaceAnalyzer();
							QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, Luggage, analyzer);
							Query qu = parser.Parse(travelOption.Luggage.ToString());
							bq.Add(qu, Occur.SHOULD);
						}
						#endregion Other
						var collector = TopScoreDocCollector.Create(searcher.MaxDoc, true);
						searcher.Search(bq, collector);

						hits = collector.TopDocs();

						maxDocs = collector.TotalHits;

						foreach (var hit in hits.ScoreDocs)
						{
							Document doc = searcher.Doc(hit.Doc);

							results.Add(Int32.Parse(doc.Get(IdField)));
						}
						return results;
					}
					catch (Exception exception)
					{
						throw new Exception(exception.Message) { };
					}
				}
			return new List<int>();
		}

		internal static Document ToDocument(Data.Travel travel)
		{
			var doc = new Document();

			doc.Add(new Field(IdField, travel.Id.ToString(),
				Field.Store.YES,
				Field.Index.NOT_ANALYZED));

			foreach (var i in travel.MapPoints)
			{
				doc.Add(new Field(MapPoints, i.Name.Replace(",", " ") ?? String.Empty, Field.Store.YES, Field.Index.ANALYZED));
				doc.Add(new Field(MapPointsLat, i.Latitude.ToString(), Field.Store.YES, Field.Index.ANALYZED));
				doc.Add(new Field(MapPointsLon, i.Longitude.ToString(), Field.Store.YES, Field.Index.ANALYZED));
			}

			doc.Add(new Field(Direction, travel.Luggage.ToString(), Field.Store.YES, Field.Index.ANALYZED));
			//doc.Add(new Field(Direction, travel.Wait.ToString(), Field.Store.YES, Field.Index.ANALYZED));
			//doc.Add(new Field(Direction, travel.Early.ToString(), Field.Store.YES, Field.Index.ANALYZED));
			doc.Add(new Field(Direction, travel.Capacity.ToString(), Field.Store.YES, Field.Index.ANALYZED));
			doc.Add(new NumericField(StartDate, Field.Store.YES, true).SetIntValue(int.Parse(travel.Startdate.ToString(DateFormate))));
			doc.Add(new NumericField(EndDate, Field.Store.YES, true).SetIntValue(int.Parse(travel.Enddate.ToString(DateFormate))));
			return doc;
		}

		public Data.Travel Add(Data.Travel doc)
		{
			if (doc == null)
			{
				throw new System.ArgumentNullException("doc");
			}
			if (IndexReader.IndexExists(_provider.IndexDirectory))
				using (IndexWriter iw = new IndexWriter(_provider.IndexDirectory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), false, IndexWriter.MaxFieldLength.UNLIMITED))
				{
					iw.AddDocument(ToDocument(doc));
				}
			return doc;
		}

		public Data.Travel Update(Data.Travel doc)
		{
			if (doc == null)
			{
				throw new System.ArgumentNullException("doc");
			}
			if (IndexReader.IndexExists(_provider.IndexDirectory))
				using (IndexWriter iw = new IndexWriter(_provider.IndexDirectory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), false, IndexWriter.MaxFieldLength.UNLIMITED))
				{
					iw.UpdateDocument(new Term(IdField, doc.Id.ToString()), ToDocument(doc));
					iw.Optimize();
				}

			return doc;
		}

		public void Delete(int id)
		{
			if (IndexReader.IndexExists(_provider.IndexDirectory))
				using (IndexWriter iw = new IndexWriter(_provider.IndexDirectory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), false, IndexWriter.MaxFieldLength.UNLIMITED))
				{
					iw.DeleteDocuments(new Term(IdField, id.ToString()));
					iw.Optimize();
				}
		}

	}
}
