using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Common.Enums;

namespace Travel.Data
{
	public class DriverComment
	{
		public string Avatar { get; set; }
		public string Message { get; set; }
		public string Email { get; set; }
		public string DisplayName { get; set; }
		public DateTime Data { get; set; }
		public int Type { get; set; }
	}
}
