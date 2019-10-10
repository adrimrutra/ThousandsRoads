using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Linq;
using Travel.Service.Services.Abstractions;
using Travel.Common.Enums;
using Travel.Web.Api.Authorization;
using System.Web.Mvc;
using System.Web;
using Travel.Authentication;
using Travel.Core;
namespace Travel.Web.Filters
{
	public class AuthenticationFilter : System.Web.Http.AuthorizeAttribute
	{
		public IAuthentification _authentification;
		//private IUserService _service;

		public override void OnAuthorization(HttpActionContext actionContext)
		{
			//_authentification = (IAuthentification)((Autofac.Core.Container)ApplicationContainer.Container).GetService(typeof(IAuthentification));
			_authentification = (IAuthentification)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IAuthentification));
			if (IsAuthorize(actionContext))
				return;
			HandleUnauthorizedRequest(actionContext);
		}

		protected bool IsAuthorize(HttpActionContext actionContext)
		{
			GetAuthAccount(actionContext);

			var authorizeAttr = GetActionControllerAttribute<OauthAuthorizeAttribute>(actionContext);
			if (authorizeAttr != null)
				return IsValidOauthCall(actionContext);

			var maybeAuthorizeAttr = GetActionControllerAttribute<MaybeAuthorizeAttribute>(actionContext);
			if (maybeAuthorizeAttr != null)
			{
				IsValidOauthCall(actionContext);
				return true;
			}

			var nonAuthorizeAttr = GetActionControllerAttribute<NonAuthorizeAttribute>(actionContext);
			if (nonAuthorizeAttr != null)
				return true;

			return true;
		}

		private bool IsValidOauthCall(HttpActionContext actionContext)
		{
			return IsAuthenticationCall();
		}

		private void GetAuthAccount(HttpActionContext actionContext)
		{
			var authHeader = actionContext.Request.Headers.Authorization;
			if (authHeader == null)
			{
				_authentification.Token = null;
				_authentification.Create = null;
				_authentification.ThUser = null;
				_authentification.User = null;
				return;
			}
			if (!string.IsNullOrEmpty(authHeader.Scheme) && !string.IsNullOrEmpty(authHeader.Parameter))
			{
				if (authHeader.Scheme == "fb")
				{
					_authentification.Token = authHeader.Parameter;
					_authentification.Create = FacebookFactory.Create;
					_authentification.Type = TokenType.Facebook;
				}
				else if (authHeader.Scheme == "fake")
				{
					_authentification.Token = authHeader.Parameter;
					_authentification.Create = FakeFactory.Create;
					_authentification.Type = TokenType.Fake;
				}
			}
		}

		private bool IsAuthenticationCall()
		{
			var _service = (IUserService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IUserService));
			if (_authentification.User == null)
				return false;

			var dbuser = _service.Get(_authentification.User.Id.ToString(), TokenType.Facebook);
			if (dbuser != null && dbuser.Id != 0)
			{
				_authentification.ThUser = dbuser;
				return (_authentification.User.IsAuthorization = true);
			}

			return false;
		}

		private TAttr GetActionControllerAttribute<TAttr>(HttpActionContext actionContext, bool inheritFromController = false)
			where TAttr : Attribute
		{
			if (actionContext == null)
				throw new ArgumentException("actionContext");
			TAttr attr = actionContext.ControllerContext.ControllerDescriptor
				.GetCustomAttributes<TAttr>(inheritFromController).SingleOrDefault();
			if (attr != null)
				return attr;
			attr = actionContext.ActionDescriptor.GetCustomAttributes<TAttr>().SingleOrDefault();
			return attr;
		}
	}
}