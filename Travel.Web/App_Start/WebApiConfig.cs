using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Web.Http;
using Travel.Web.Filters;
namespace Travel.Web
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();
			config.Filters.Clear();
			config.Filters.Add(new AuthenticationFilter());
			//config.Routes.MapHttpRoute(
			//	name: "DefaultApi",
			//	routeTemplate: "api/{controller}/{id}",
			//	defaults: new { id = RouteParameter.Optional }
			//);
		}
	}
}
