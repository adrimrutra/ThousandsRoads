using Travel.Common.Enums;

namespace Travel.Data
{
	public class Traveler
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		public int TravelId { get; set; }
		public Travel Travel { get; set; }
		public UserType? Usertype { get; set; }		
	}
}
