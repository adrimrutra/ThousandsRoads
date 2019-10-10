using System.Data.Entity.ModelConfiguration;
using Travel.Entity;

namespace Travel.Migration.Configuration
{
	public class LuggageConfiguration: EntityTypeConfiguration<Luggage>
	{
		public LuggageConfiguration()
		{
			HasKey(x => x.Id);
		}
	}
}
