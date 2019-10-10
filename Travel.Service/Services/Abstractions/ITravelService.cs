using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel.Service.Services.Abstractions
{
	public interface ITravelService : IService<Travel.Data.Travel>
	{
		bool PutTraveler(int DriverId, int TravelId, int UserId);
		IEnumerable<Data.Travel> GetTravelsByIds(IEnumerable<int> Ids);
	}
}
