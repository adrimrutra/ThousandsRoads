using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Travel.Service.Services.Abstractions;
using Travel.Web.Api.Controllers.Base;
using System.Net.Http;
using System.Net;
using Travel.Web.Api.Authorization;
using Travel.Service.Search;
using Travel.Data;
using Travel.Authentication;
using Travel.Core;

namespace Travel.Web.Api.Controllers
{
	[NonAuthorize]
	[RoutePrefix("api/travel")]
	public class TravelController : ApiControllerBase<Data.Travel>
	{
		#region Dependency
		private ITravelService _service;
		private ISearchTravelService _searchService;
		private IAuthentification _auth;
		private IUserService _userService;
		public TravelController(ITravelService service, ISearchTravelService searchService, IAuthentification auth)
		{
			this._service = service;
			this._searchService = searchService;
			//this._auth = auth;
			_auth = (IAuthentification)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IAuthentification));
		}
		#endregion Dependency

		[Route("")]
		public IEnumerable<Data.Travel> GetAll([FromUri]SearchTravel sInfo = null)
		{
			if (sInfo == null)
				return this._service.GetAll();
			else
			{
				var ids = this._searchService.Search(sInfo);
				return this._service.GetTravelsByIds(ids);
			}
		}

		[Route("{id:int}")]
		public override Data.Travel Get(int Id)
		{
			return this._service.Get(Id);
		}

		[OauthAuthorize]
		[Route("{id:int}")]
		public override void Delete(int Id)
		{
			this._service.Delete(Id);
			this._searchService.Delete(Id);
		}

		[OauthAuthorize]
		[Route("")]
		public override Data.Travel Post(Data.Travel _object)
		{
			try
			{
				var travel = this._service.Add(_object);
				this._searchService.Add(travel);
				this._searchService.Update(travel);
				return travel;
			}
			catch (ArgumentException ex)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		[OauthAuthorize]
		[Route("{id:int}")]
		public override Data.Travel Put(int Id, Data.Travel _object)
		{
			_object.Id = Id;
			var travel = this._service.Save(_object);
			this._searchService.Update(travel);
			return travel;
		}
		
		[OauthAuthorize]
		[Route("traveler")]
		public bool GetTraveler([FromUri] int TravelId, [FromUri] int UserId)
		{
			if (_auth != null && _auth.Token != null)
			{
				if (_auth.ThUser == null)
					_auth.ThUser = _userService.Get(_auth.User.Id.ToString(), _auth.Type);
			}
			return this._service.PutTraveler(_auth.ThUser.Id, TravelId, UserId);
		}
	}
}
