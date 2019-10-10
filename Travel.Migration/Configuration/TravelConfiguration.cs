using System.Data.Entity.ModelConfiguration;

namespace Travel.Migration.Configuration
{
	class TravelConfiguration : EntityTypeConfiguration<Travel.Entity.Travel>
	{
		public TravelConfiguration()
		{
			HasKey(x => x.Id);
		}
	}
}
