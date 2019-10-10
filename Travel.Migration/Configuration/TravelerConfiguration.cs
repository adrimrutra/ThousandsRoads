using System.Data.Entity.ModelConfiguration;
using Travel.Entity;

namespace Travel.Migration.Configuration
{
	class TravelerConfiguration : EntityTypeConfiguration<Traveler>
	{
		public TravelerConfiguration()
		{
			HasKey(x => x.Id);

			HasRequired<User>(x => x.User)
				.WithMany(x => x.Travelers)
				.HasForeignKey(x => x.UserId)
				.WillCascadeOnDelete();

			HasOptional<Entity.Travel>(x => x.Travel)
				.WithMany(x => x.Travelers)
				.HasForeignKey(x => x.TravelId);
		}
	}

}
