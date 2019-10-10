using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Data;

namespace Travel.Service.Search
{
	public interface ISearchTravelService
	{
		IEnumerable<int> Search(SearchTravel travelOption);
		Travel.Data.Travel Add(Travel.Data.Travel doc);
		Travel.Data.Travel Update(Travel.Data.Travel doc);
		void Delete(int id);
	}
}
