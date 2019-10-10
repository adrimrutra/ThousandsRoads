using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Travel.Authentication;
using Travel.Core.Exceptions;
using Travel.Data;
using Travel.Service.Services.Abstractions;
using Travel.Web.Api.Authorization;
using Travel.Web.Api.Controllers.Base;
using System.Net.Http;
using Travel.Core;

namespace Travel.Web.Api.Controllers
{
	[RoutePrefix("api/comment")]
	public class CommentController : ApiControllerBase<Data.Comment>
	{
		#region Dependency
		private ICommentService _commentService;
		private IAuthentification _auth;
		private IUserService _userService;
		public CommentController(ICommentService service, IAuthentification auth, IUserService userService)
		{
			_commentService = service;
			//_auth = auth;
			_auth = (IAuthentification)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IAuthentification));
			_userService = userService;
		}
		#endregion Dependency

		[Route("")]
		public IEnumerable<Data.Comment> GetAll([FromUri]int userId)
		{
			return this._commentService.GetAll(userId);
		}

		[Route("")]
		public Data.Comment Get([FromUri]int Id, [FromUri]int userId)
		{
			return this._commentService.Get(Id, userId);
		}

		[OauthAuthorize]
		[Route("")]
		public override Data.Comment Post(Data.Comment _object)
		{
			try
			{
				_object.Messenger = _auth.ThUser;
				_object.MessengerId = _auth.ThUser.Id;
				if (_auth != null && _auth.Token != null)
				{
					if (_auth.ThUser == null)
						_auth.ThUser = _userService.Get(_auth.User.Id.ToString(), _auth.Type);
				}
				_object.User = _userService.Get(_object.UserId);
				if (_object.User == null)
					throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ""));
				return _commentService.Add(_object);
			}
			catch (SomethingWrongException ex)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
			}
			catch (NotAuthorizedException ex)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, ex.Message));
			}

		}

		[Route("{id:int}")]
		public override Comment Get(int Id)
		{
			throw new NotImplementedException();
		}

		[Route("{id:int}")]
		public override void Delete(int Id)
		{
			throw new NotImplementedException();
		}

		[Route("{id:int}")]
		public override Comment Put(int Id, Comment _object)
		{
			throw new NotImplementedException();
		}
	}
}
