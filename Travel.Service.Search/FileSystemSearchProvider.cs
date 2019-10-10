using Lucene.Net.Index;
using Lucene.Net.Store;
using System;
using Travel.Core;

namespace Travel.Service.Search
{
	public class FileSystemSearchProvider : ISearchProvider
	{
		private readonly string IndexDirPath = LuceneDirection.Path;
		
		public Lucene.Net.Store.Directory IndexDirectory { get; private set; }

		public FileSystemSearchProvider()
		{
			if (!System.IO.Directory.Exists(IndexDirPath))
			{
				System.IO.Directory.CreateDirectory(IndexDirPath);
			}
			IndexDirectory = FSDirectory.Open(IndexDirPath);
			
		}

		public bool NeedsIndexRebuildAtStartup
		{
			get { return false; }
		}


		public DateTimeOffset LastModified
		{
			get {
				return new DateTime(1970, 1, 1).AddMilliseconds(IndexReader.LastModified(IndexDirectory)).ToLocalTime();
			}
		}
	}
}
