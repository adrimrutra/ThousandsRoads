using System.Collections.Generic;

namespace Travel.Entity
{
	public class FriendsList
	{
		public int Id { get; set; }
		public ICollection<User> Users { get; set; }
		public FriendListItem FriendListItem { get; set; }
	}
}
