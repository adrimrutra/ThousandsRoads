using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Travel.Web.Root;

namespace Travel.Web
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		protected static AutofacResolver resolver;
		public static AutofacResolver Resolver { get { return resolver; } }
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			GlobalConfiguration.Configuration.EnsureInitialized();
			GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
			resolver = new AutofacResolver();
		}
		protected void Application_Error(object sender, EventArgs args)
		{
			Exception ex = Server.GetLastError();
			if (ex == null)
				_logger.Error("Unknown Error");
			else
				_logger.ErrorException("Error-> ", ex);
		}
	}
}
