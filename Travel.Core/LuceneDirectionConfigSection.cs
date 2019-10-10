using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace Travel.Core
{
	class LuceneDirectionConfigSection : ConfigurationSection
	{
		[ConfigurationProperty("Path", IsRequired = true)]
		public string Path
		{
			get
			{
				return (string)this["Path"];
			}
		}
	}
	public class LuceneDirection
	{
		public static string Path
		{
			get
			{
				var section = WebConfigurationManager.GetSection("LuceneDirection") as LuceneDirectionConfigSection;
				if(System.IO.Path.IsPathRooted(section.Path))
					return section.Path;
				var combine = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, section.Path);
				return System.IO.Path.GetFullPath(combine);
			}
		}

	}
}
