using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Common.Enums;

namespace Travel.Entity
{
	public class Message
	{
		public int Id { get; set; }

		public User User { get; set; }
		public int? UserId { get; set; }

		public User Messenger { get; set; }
		public int? MessengerId { get; set; }

		public Travel Travel { get; set; }
		public int? TravelId { get; set; }

		public string Theme { get; set; }
		public string MessageText { get; set; }
		public bool State { get; set; }
		public DirectionType Direction { get; set; }
		public LuggageType Luggage { get; set; }
		public int PersonCount { get; set; }
		public MessageType Type { get; set; }
	}
}
