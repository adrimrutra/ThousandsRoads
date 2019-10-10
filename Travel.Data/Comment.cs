using System;
using Travel.Common.Enums;

namespace Travel.Data
{
	public class Comment
	{
		public int UserId { get; set; }
		public User User { get; set; }
		public int MessengerId { get; set; }
		public User Messenger { get; set; }
		public string Message { get; set; }
		public CommentType Type { get; set; }
		public DateTimeOffset? Data { get; set; }
	}
}
