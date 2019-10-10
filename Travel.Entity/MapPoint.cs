
namespace Travel.Entity
{
	public class MapPoint
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public Travel Travel { get; set; }
		public int TravelId { get; set; }
	}
}
