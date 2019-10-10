using System.Collections.Generic;

namespace Travel.Data
{
	public class User
	{
		public User()
		{
			Tokens = new List<Token>();
			Comments = new List<Comment>();
			Travelers = new List<Traveler>();
			FriendListItems = new List<FriendListItem>();
		}
		public int Id { get; set; }
		public int? Rating { get; set; }
		public string DisplayName { get; set; }
		public ICollection<Token> Tokens { get; set; }
		public ICollection<FriendListItem> FriendListItems { get; set; }
		public string Avatar { get; set; }
		public string Email { get; set; }
		public ICollection<Comment> Comments { get; set; }
		public ICollection<Traveler> Travelers { get; set; }

	}
}
