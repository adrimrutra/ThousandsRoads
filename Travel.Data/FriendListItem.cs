
namespace Travel.Data
{
    public class FriendListItem
    {
		public int Id { get; set; }
		public FriendsList CustomerList { get; set; }
		public User Owner { get; set; }
		public int OwnerId { get; set; }
    }
}
