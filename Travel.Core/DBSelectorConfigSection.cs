using System.Configuration;
using System.Web.Configuration;


namespace Travel.Core
{
	public class DBSelectorConfigSection : ConfigurationSection
	{
		[ConfigurationProperty("IsMSSQL", IsRequired = true)]
		public bool IsMSSQL
		{
			get
			{
				return (bool)this["IsMSSQL"];
			}
		}
	}
	public class DbChoice
	{
		public static bool IsMSSQ
		{
			get
			{
				var section = WebConfigurationManager.GetSection("DBSelector") as DBSelectorConfigSection;
				return section.IsMSSQL;
			}
		}

	}
}
