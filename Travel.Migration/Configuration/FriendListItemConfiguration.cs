using System.Data.Entity.ModelConfiguration;
using Travel.Entity;

namespace Travel.Migration.Configuration
{
	public class FriendListItemConfiguration : EntityTypeConfiguration<FriendListItem>
	{
		public FriendListItemConfiguration()
		{
			HasKey(x => x.Id);

			HasMany(x => x.Users)
				.WithMany(x => x.FriendListItems);

			HasOptional(x => x.Owner)
				.WithMany()
				.HasForeignKey(x => x.OwnerId);

			HasOptional(x => x.CustomerList)
				.WithRequired(x => x.FriendListItem)
				.Map(x => x.MapKey("FriendList"));
		}
	}
}
