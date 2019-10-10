
namespace Travel.Data
{
	public class Luggage
	{
		public int? TravelId { get; set; }
		public Travel Travel { get; set; }
		public int? UserId { get; set; }
		public User User { get; set; }
		public string Startpoint { get; set; }
		public string Endpoint { get; set; }
	}
}
