using System.Data.Entity.ModelConfiguration;
using Travel.Entity;

namespace Travel.Migration.Configuration
{
	class CommentConfiguration : EntityTypeConfiguration<Comment>
	{
		public CommentConfiguration()
		{
			HasKey(x => x.Id);

			HasRequired<User>(x => x.User)
				.WithMany(x => x.Comments)
				.HasForeignKey(x => x.UserId)
				.WillCascadeOnDelete(false);

			HasRequired<User>(x => x.Messenger)
				.WithMany()
				.HasForeignKey(x => x.MessengerId)
				.WillCascadeOnDelete(false);
		}

	}
}
