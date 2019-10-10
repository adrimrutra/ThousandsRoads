using System.Collections.Generic;

namespace Travel.Data
{
	public class FriendsList
	{
		public FriendsList()
		{
			Customers = new List<User>();
		}
		public int Id { get; set; }
		public ICollection<User> Customers { get; set; }
		public FriendListItem ParentList { get; set; }
		public int ParentListId { get; set; }
	}
}
