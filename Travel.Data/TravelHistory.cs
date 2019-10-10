using System;
using Travel.Common.Enums;

namespace Travel.Data
{
	public class TravelHistory
	{
		public int UserId { get; set; }
		public User User { get; set; }
		public UserType Usertype { get; set; }
		public DateTime Date { get; set; }
	}
}
