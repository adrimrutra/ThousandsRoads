using System.Collections.Generic;

namespace Travel.Data
{
	public class TravelFriends
	{
		public int TravelId { get; set; }
		public IEnumerable<MyFriend> Friends { get; set; }
	}
}
