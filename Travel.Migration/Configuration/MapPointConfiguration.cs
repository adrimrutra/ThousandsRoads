using System.Data.Entity.ModelConfiguration;
using Travel.Entity;

namespace Travel.Migration.Configuration
{
	class MapPointConfiguration : EntityTypeConfiguration<MapPoint>
	{
		public MapPointConfiguration()
		{
			HasKey(x => x.Id);

			HasRequired<Travel.Entity.Travel>(x => x.Travel)
				.WithMany(x => x.MapPoints)
				.HasForeignKey(x => x.TravelId)
				.WillCascadeOnDelete();
		}
	}
}
