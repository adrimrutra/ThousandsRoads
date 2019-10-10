using System.Configuration;
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
				var section = WebConfigurationManager.GetSection("InterSystems") as InterSystemsSection;
				return section.ConnectionString;
			}
		}

	}
}
