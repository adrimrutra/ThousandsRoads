using System.Data.Entity.ModelConfiguration;
using Travel.Entity;

namespace Travel.Migration.Configuration
{
	class MessageConfiguration : EntityTypeConfiguration<Message>
	{
		public MessageConfiguration()
		{
			HasKey(x => x.Id);

			HasOptional(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId);

			HasOptional(x => x.Travel)
				.WithMany()
				.HasForeignKey(x => x.TravelId);

			HasOptional(x => x.Messenger)
				.WithMany()
				.HasForeignKey(x => x.MessengerId);
		}

	}
}
