namespace Travel.Migrations
{
    using Travel.Service;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TravelContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

        }

		protected override void Seed(TravelContext context)
		{
			#region Templaite
			//  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //context.Authors.AddOrUpdate(
            //        new Author { DisplayName = "1 Author" },
            //        new Author { DisplayName = "2 Author" , DeletedDate =  new System.DateTimeOffset(DateTime.Now)},
            //        new Author { DisplayName = "3 Author" },
            //        new Author { DisplayName = "4 Author" },
            //        new Author { DisplayName = "5 Author" }
            //    );

            //context.Articles.AddOrUpdate(
            //        new Article { Title = "1 Article", PublicationId =1},
            //        new Article { Title = "2 Article", PublicationId = 1, DeletedDate = new System.DateTimeOffset(DateTime.Now) },
            //        new Article { Title = "3 Article", PublicationId = 1 },
            //        new Article { Title = "4 Article", PublicationId = 1 },
            //        new Article { Title = "5 Article", PublicationId = 1 }
			//    );
			#endregion Templaite
		}
    }
}
