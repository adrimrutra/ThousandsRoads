using Travel.Common.Enums;

namespace Travel.Entity
{
	public class Token
	{
		public int Id { get; set; }
		public TokenType Tokentype { get; set; }
		public string SocialId { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
