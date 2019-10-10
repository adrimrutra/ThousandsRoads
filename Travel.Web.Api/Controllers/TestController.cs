using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Travel.Authentication;
using Travel.Service.Services.Abstractions;
using Travel.Web.Api.Authorization;
using Travel.Web.Api.Controllers.Base;
using Travel.Common.Enums;
using Travel.Data;
using Travel.Service.Search;

namespace Travel.Web.Api.Controllers
{
	[RoutePrefix("api/test")]
	public class TestController : ApiControllerBase<Object>
	{
		#region Dependency
		private IUserService _service
			;
		private IAuthentification _auth;
		private ISearchIndexService _search;
		public TestController(IUserService service, IAuthentification auth, ISearchIndexService search)
		{
			this._service = service;
			_auth = auth;
			_search = search;
		}
		#endregion Dependency

		[Route("search/rebuild")]
		public void GetRebuild([FromUri] string rebuildKey)
		{
			if (rebuildKey == "[Sigm@]vm")
			{
				_search.RebuildIndex();
			}
		}

		[Route("friend/get")]
		public IEnumerable<MyFriend> GetFriend()
		{
			var list = GetTokens();
			foreach (var token in list)
			{
				_auth = new Authentification();
				_auth.Create = FakeFactory.Create;
				_auth.Token = token;
				_auth.Type = TokenType.Fake;
				var user = _auth.User;
				var friends = user.GetFriends();
				var Tokentype = TokenType.Fake;
				var SocialId = long.Parse(user.Name.Split(new char[] { ':' }).First()).ToString();
				_auth.ThUser = _service.Get(SocialId, Tokentype);
				return _service.GetFriends(_auth.ThUser.Id, 32);
			}
			return null;
		}

		[Route("friend/update")]
		public void GetUpdateFriend()
		{
			var list = GetTokens();
			foreach (var token in list)
			{
				_auth = new Authentification();
				_auth.Create = FakeFactory.Create;
				_auth.Token = token;
				_auth.Type = TokenType.Fake;
				var user = _auth.User;
				var friends = user.GetFriends();
				var thUser = new Data.User
				{
					Tokens = new List<Data.Token>{
                    new Data.Token{
                        Tokentype = TokenType.Fake,
                        SocialId = long.Parse( user.Name.Split(new char[] { ':' }).First()).ToString()
                    }
                },
					DisplayName = user.Name
				};
				_auth.ThUser = _service.Add(thUser);
				_service.UpdateFriedsList(_auth);
			}
		}
		#region Base
		public override object Get(int Id)
		{
			throw new NotImplementedException();
		}

		public override void Delete(int Id)
		{
			throw new NotImplementedException();
		}

		public override object Post(object _object)
		{
			throw new NotImplementedException();
		}

		public override object Put(int Id, object _object)
		{
			throw new NotImplementedException();
		}
		#endregion

		public List<string> GetTokens()
		{
			return new List<string> {
                "000001:user1;000002:user2;000005:user5;",
                "000002:user2;000003:user3;000001:user1;",
                "000003:user3;000002:user2;000005:user5;",
                "000004:user4;000003:user3;000005:user5;",
                "000005:user5;000001:user1;000003:user3;"
            };
		}
	}
}
