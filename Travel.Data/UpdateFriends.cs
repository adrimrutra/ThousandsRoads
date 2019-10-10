using System.Collections.Generic;
using Travel.Common.Enums;

namespace Travel.Data
{
	public class UpdateFriends
	{
		public UpdateFriends()
		{
			FriendsOathId = new List<long>();
		}
		public int OwnerId { get; set; }
		public ICollection<long> FriendsOathId { get; set; }
		public TokenType OathProvider { get; set; }
	}
}
