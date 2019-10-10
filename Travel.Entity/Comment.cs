using System;
using Travel.Common.Enums;

namespace Travel.Entity
{
	public class Comment
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
        public int MessengerId { get; set; }
        public virtual User Messenger { get; set; }
        public string Message { get; set; }
		public CommentType Type { get; set; }
		public DateTimeOffset? Data { get; set; }
	}
}
