using Travel.Common.Enums;

namespace Travel.Data
{
	public class Message
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string UserEmail { get; set; }
		public int MessengerId { get; set; }
		public string MessengerDisplayName { get; set; }
		public string MessengerAvatar { get; set; }
		public string MessengerEmail { get; set; }
		public int TravelId { get; set; }
		public string Theme { get; set; }
		public string MessageText { get; set; }
		public bool State { get; set; }
		public DirectionType Direction { get; set; }
		public LuggageType Luggage { get; set; }
		public int PersonCount { get; set; }
		public MessageType Type { get; set; }
	}
}

 