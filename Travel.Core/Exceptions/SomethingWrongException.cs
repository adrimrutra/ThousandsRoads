using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel.Core.Exceptions
{
	public class SomethingWrongException: Exception
	{
		protected static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		public SomethingWrongException(Exception ex)
		{
			_logger.ErrorException("Error-> ", ex);
		}
	
	}
}
