using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Entity;
using Travel.Migration.Configuration;
using Travel.Service.Configuration;

namespace Travel.Service
{
	public class TravelContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public TravelContext()
			: base("ThRoadsEntityDB")
		{
			this.Configuration.LazyLoadingEnabled = false;
		}
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add<User>(new UserConfiguration());

            modelBuilder.Configurations.Add<Comment>(new CommentConfiguration());

			modelBuilder.Configurations.Add<Luggage>(new LuggageConfiguration());

			modelBuilder.Configurations.Add<MapPoint>(new MapPointConfiguration());

			modelBuilder.Configurations.Add<Travel.Entity.Travel>(new TravelConfiguration());

			modelBuilder.Configurations.Add<Token>(new TokenConfiguration());

			modelBuilder.Configurations.Add<Traveler>(new TravelerConfiguration());

			modelBuilder.Configurations.Add<FriendsList>(new FriendListConfiguration());

			modelBuilder.Configurations.Add<FriendListItem>(new FriendListItemConfiguration());

            modelBuilder.Configurations.Add<Message>(new MessageConfiguration());
		}

	}
}
