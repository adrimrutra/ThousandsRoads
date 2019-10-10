using System.Data.Entity.ModelConfiguration;
using Travel.Entity;

namespace Travel.Migration.Configuration
{
	class TokenConfiguration: EntityTypeConfiguration<Token>
	{
		public TokenConfiguration()
		{
			HasKey(x => x.Id);

			Property(x => x.Tokentype)
				.IsRequired();

			HasRequired<User>(x => x.User)
				.WithMany(x => x.Tokens)
				.HasForeignKey(x => x.UserId)
				.WillCascadeOnDelete();
		}
	}
}
