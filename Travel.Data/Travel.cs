using System;
using System.Collections.Generic;
using Travel.Common.Enums;

namespace Travel.Data
{
	public class Travel
	{
		public Travel()
		{
			MapPoints = new List<MapPoint>();
			Travelers = new List<Traveler>();
		}
		public int Id { get; set; }
		public int DriverId { get; set; }
		public int? Capacity { get; set; }
		public ICollection<Traveler> Travelers { get; set; }
		public ICollection<MapPoint> MapPoints { get; set; }
		public DateTime Startdate { get; set; }
		public DateTime Enddate { get; set; }
		public string Startpoint { get; set; }
		public string Endpoint { get; set; }
		public string DisplayName { get; set; }
		public string Mapsnapshot { get; set; }
		public LuggageType Luggage { get; set; }
	}
}
