using System;
using System.Collections.Generic;
using Travel.Common.Enums;

namespace Travel.Data
{
	public class SearchTravel
	{
		public string Saddress { get; set; }
		public double? Slat { get; set; }
		public double? Slon { get; set; }
		public DateTimeOffset? Sdate { get; set; }
		public string Eaddress { get; set; }
		public double? Elat { get; set; }
		public double? Elon { get; set; }
		public DateTimeOffset? Edate { get; set; }

		public DirectionType? Direction { get; set; }
		public LuggageType? Luggage { get; set; }
		public WaitType? Wait { get; set; }
		public EarlyType? Early { get; set; }
		public int? Capacity { get; set; }
	}
}
