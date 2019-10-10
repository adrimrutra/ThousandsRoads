
using System.Collections.Generic;
namespace Travel.Entity
{
	public class FriendListItem
	{
		public int Id { get; set; }
		public FriendsList CustomerList { get; set; }
		public ICollection<User> Users { get; set; }
		public User Owner { get; set; }
		public int? OwnerId { get; set; }
	}
}
