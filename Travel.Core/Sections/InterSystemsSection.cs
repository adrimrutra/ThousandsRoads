using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Travel.Core
{
	public class InterSystemsSection : ConfigurationSection
	{
		[ConfigurationProperty("ConnectionString", IsRequired = true)]
		public string ConnectionString
		{
			get
			{
				return (string)this["ConnectionString"];
			}
		}
	}
	public class Cache
	{
		public static string ConnectionString
		{
			get
			{
                return "Server=localhost; Namespace=USER; Password=SYS; User ID=_SYSTEM;";
               
                    //var section = WebConfigurationManager.GetSection("InterSystems") as InterSystemsSection;
                    //return section.ConnectionString;
			}
		}

	}
}
