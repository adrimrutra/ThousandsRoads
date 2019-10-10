using System.Data.Entity.ModelConfiguration;
using Travel.Entity;

namespace Travel.Migration.Configuration
{
	public class FriendListConfiguration : EntityTypeConfiguration<FriendsList>
	{
		public FriendListConfiguration()
		{
			HasKey(x => x.Id);

			HasMany(x => x.Users)
				.WithMany();
		}
	}
}
