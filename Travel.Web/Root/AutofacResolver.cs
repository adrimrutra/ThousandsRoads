using System;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Reflection;

using Autofac;
using Autofac.Integration.WebApi;
using Autofac.Integration.Mvc;

using Travel.Migration;
using Travel.Core.Data;
using Travel.Service;
using Travel.Service.Mapping;
using Travel.Service.Services;
using Travel.Web.Api.Controllers.Base;
using Travel.Web.Controllers;

using Travel.Service.Services.Abstractions;

using InterSystems.Data.CacheTypes;
using Travel.Core;

using System.Configuration;
using Travel.Web.Mail;
using Travel.Authentication;

using Travel.Service.Search;

namespace Travel.Web.Root
{
	public class AutofacResolver : IServiceProvider
	{
		private readonly IContainer container;
		public IContainer Container { get { return this.container; } }

		public AutofacResolver()
		{
			this.container = GetContainer();
		}

		private IContainer GetContainer()
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<Mailer>()
				.As<IMailer>()
				.InstancePerHttpRequest();

			builder.RegisterType<Authentification>()
				.AsImplementedInterfaces()
				.InstancePerHttpRequest();

			builder.RegisterType<SearchTravelService>()
				.As<ISearchTravelService>()
				.InstancePerHttpRequest();

			builder.RegisterType<FileSystemSearchProvider>()
				.As<ISearchProvider>()
				.InstancePerLifetimeScope();

			builder.RegisterType<SearchIndexService>()
				.As<ISearchIndexService>()
				.InstancePerLifetimeScope();

			if (DbChoice.IsMSSQ)
			{
				builder.RegisterType<DbSession>()
					.As<IDbSession>()
					.InstancePerLifetimeScope();
				builder.RegisterType<TravelContext>()
					.As<DbContext>()
					.InstancePerLifetimeScope();

				var serviceAssembly = Assembly.GetAssembly(typeof(IService<>));
				//	var serviceAssembly=Assembly.GetAssembly()
				builder.RegisterAssemblyTypes(serviceAssembly)
					.Where(x => x.IsClass && x.Name.EndsWith("Service"))
					.AsImplementedInterfaces()
					.InstancePerLifetimeScope();

				builder.RegisterType<EmitObjectMapper>()
					.As<IObjectMapper>()
					.SingleInstance();
			}
			



			var apiControllerAssembly = Assembly.GetAssembly(typeof(ApiControllerBase<>));

			builder.RegisterAssemblyTypes(apiControllerAssembly)
				.InstancePerHttpRequest();

			var mvcControllerAssembly = Assembly.GetAssembly(typeof(MvcControllerBase));

			builder.RegisterAssemblyTypes(mvcControllerAssembly)
				.InstancePerHttpRequest();


			var container = builder.Build();
			GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
			ApplicationContainer.Container = container;
			return container;
		}
		public object GetService(Type serviceType)
		{
			return container.Resolve(serviceType);
		}
	}
}