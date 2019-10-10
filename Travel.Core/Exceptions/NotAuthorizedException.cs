using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel.Core.Exceptions
{
	public class NotAuthorizedException : Exception
	{
		public NotAuthorizedException()
		{ }
		public NotAuthorizedException(string message)
			: base(message)
		{ }
	}
}
