using System.Data.Entity.ModelConfiguration;
using Travel.Entity;

namespace Travel.Service.Configuration
{
	class UserConfiguration : EntityTypeConfiguration<User>
	{
		public UserConfiguration()
		{
			HasKey(x => x.Id);

			HasMany(x => x.FriendListItems)
				.WithMany(x => x.Users);
		}
	}
}
