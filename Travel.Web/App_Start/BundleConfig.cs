using System.Web;
using System.Web.Optimization;

namespace Travel.Web
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js",
						"~/Scripts/jquery-ui-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css",
					  "~/Content/Travel.css",
					  "~/Content/Font.css",
					  "~/Content/Cabinet.css",
					  "~/Content/CallBack.css",
					  "~/Content/bootstrap-responsive.css",
					  "~/Content/th-bootstrap.css",
					  "~/Content/bootstrap-datetimepicker/datetimepicker.css"				  
					  ));

			bundles.Add(new ScriptBundle("~/oauth").Include(
					  "~/Scripts/oAuth/oauth.js",
					  "~/Scripts/oAuth/Account.js"
				));

			bundles.Add(new ScriptBundle("~/bundles/angular").Include(
					  "~/Scripts/angular.js",
					  "~/Scripts/angular-resource.js",
					  "~/Scripts/angular-route.js",
					  "~/Scripts/uiAngular/draggable.js",
					  "~/Scripts/uiAngular/ui-event.js",
					  "~/Scripts/bootstrap-datetimepicker/bootstrap-datetimepicker.js",
					  "~/Scripts/uiAngular/ui-map.js",
					  "~/Scripts/ui-bootstrap-0.10.0.js",
					  "~/Scripts/ui-bootstrap-tpls-0.10.0.js",
					  "~/Scripts/angular-cookies.js",
					  "~/Scripts/angular-strap.js"
					  
				));



			bundles.Add(new ScriptBundle("~/bundles/Application").Include(
					 "~/Scripts/Application/Models/Travel.js",
					 "~/Scripts/Application/Models/Traveler.js",
					 "~/Scripts/Application/Models/Comment.js",
					 "~/Scripts/Application/Models/Point.js",
					 "~/Scripts/Application/Models/User.js",
					 "~/Scripts/Application/Models/FriendListItem.js",
					 "~/Scripts/Application/Models/FriendsList.js",
					 "~/Scripts/Application/Models/Token.js",
					 "~/Scripts/Application/Models/UpdateFriends.js",
					 "~/Scripts/Application/Models/DriverComment.js",
					 "~/Scripts/Application/Models/SearchTravel.js",
					 "~/Scripts/Application/Models/MyFriend.js",
					 "~/Scripts/Application/Models/TravelFriends.js",
					 "~/Scripts/Application/Models/Message.js",
					 "~/Scripts/Application/Services/Service.js",
					 "~/Scripts/Application/Services/UserService.js",
					 "~/Scripts/Application/Services/TravelService.js",
					 "~/Scripts/Application/Services/TravelerService.js",
					 "~/Scripts/Application/Services/CommentService.js",
					 "~/Scripts/Application/Controllers/TravelController.js",
					 "~/Scripts/Application/Controllers/TravelsController.js",
					 "~/Scripts/Application/Controllers/NewTravelController.js",
					 "~/Scripts/Application/Controllers/NewUserController.js",
					 "~/Scripts/Application/Controllers/ApplicationController.js",
					 "~/Scripts/Application/Controllers/AccountController.js",
					 "~/Scripts/Application/Controllers/HomeController.js",
					 "~/Scripts/Application/Controllers/SearchController.js",
					 "~/Scripts/Application/Controllers/CabinetController.js",
					 "~/Scripts/Application/Controllers/CallBackController.js",
					 "~/Scripts/GoogleMaps/ngAutocomplete.js",
					 "~/Scripts/Application/AuthInterceptor.js",


					 "~/Scripts/Application/Application.js"
				));

			bundles.Add(new ScriptBundle("~/bundles/other").Include(
					  "~/Scripts/moment.js"
				));


		}
	}
}
