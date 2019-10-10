using System.Collections.Generic;
using System.Web.Http;
using Travel.Common.Enums;
using Travel.Service.Services.Abstractions;
using Travel.Web.Api.Controllers.Base;
using Travel.Core;
using Travel.Web.Api.Authorization;
using System;
using System.Net;
using System.Net.Http;
using Travel.Core.Exceptions;
using Travel.Authentication;
using Travel.Data;
using NLog;

namespace Travel.Web.Api.Controllers
{
	[NonAuthorize]
	[RoutePrefix("api/user")]
	public class UserController : ApiControllerBase<Data.User>
	{
		#region Dependency
		private IUserService _userService;
		private IMessageService _messageService;
		private IMailer _mailer;
		private IAuthentification _auth;
		protected static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		public UserController(IUserService service, IMailer mailer, IAuthentification auth, IMessageService messageService)
		{
			this._userService = service;
			this._mailer = mailer;
			//_auth = auth;
			//_auth = (IAuthentification)((Autofac.Core.Container)ApplicationContainer.Container).GetService(typeof(IAuthentification));
			_auth = (IAuthentification)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IAuthentification));

			this._messageService = messageService;
			//_mailer.Welcome().Send();


		}
		#endregion Dependency

		[Route("")]
		public IEnumerable<Data.User> GetAll()
		{
			return this._userService.GetAll();
		}

		[Route("{id:int}")]
		public override Data.User Get(int Id)
		{
			return this._userService.Get(Id);
			// throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ""));

		}

		[OauthAuthorize]
		[Route("{id:int}")]
		public override void Delete(int Id)
		{
			//this.userService.Delete(Id);
			throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Нежданчік"));
		}

		[Route("")]
		public override Data.User Post(Data.User _object)
		{
			//_mailer.Subscription("Hello Thousands Roads", _object.Email).Send();
			var res = this._userService.Add(_object);
			_auth.ThUser = res;
			AsyncMethod updateFriend = UpdateFriend;
			updateFriend.BeginInvoke(null, null);
			return res;
		}

		delegate void AsyncMethod();

		protected void UpdateFriend()
		{
			try
			{

				if (_auth != null && _auth.Token != null)
				{
					if (_auth.ThUser == null)
						_auth.ThUser = _userService.Get(_auth.User.Id.ToString(), _auth.Type);
				}
				this._userService.UpdateFriedsList(_auth);
			}
			catch (Exception ex)
			{
				_logger.ErrorException("User Controller UpdateFriend", ex);
			}
		}

		[OauthAuthorize]
		[Route("{id:int}")]
		public override Data.User Put(int Id, Data.User _object)
		{
			_object.Id = Id;
			return this._userService.Save(_object);
		}

		[MaybeAuthorize]
		[Route("socialid/{id:long}")]
		public Data.User GetSocial(long id, [FromUri] TokenType type)
		{

			var res = this._userService.Get(id.ToString(), type);
			if (res != null)
			{
				_auth.ThUser = res;
				AsyncMethod updateFriend = UpdateFriend;
				updateFriend.BeginInvoke(null, null);
			}
			return res;
		}

		[OauthAuthorize]
		[Route("notifypassenger")]
		public bool GetNotifyPassenger([FromUri] int driver, [FromUri] int travel)
		{
			try
			{
				var tmpdriver = _userService.Get(driver);
				if (tmpdriver != null)
				{
					if (_auth != null && _auth.Token != null)
					{
						if (_auth.ThUser == null)
							_auth.ThUser = _userService.Get(_auth.User.Id.ToString(), _auth.Type);
					}
					Message message = new Message();
					message.UserId = _auth.ThUser.Id;
					message.UserEmail = _auth.ThUser.Email;
					message.MessengerId = tmpdriver.Id;
					message.MessengerDisplayName = tmpdriver.DisplayName;
					message.MessengerEmail = tmpdriver.Email;
					message.TravelId = travel;
					message.Theme = "Повідомлення про підписку";
					message.MessageText = "Ви підписались на поїздку.";
					message.Type = MessageType.Inner;
					message.State = false;
					_messageService.Add(message);
					string cabinetLink = string.Format("{0}://{1}/#/cabinet?message={2}",
							Request.RequestUri.Scheme, Request.RequestUri.Authority, message.Id);
					_mailer.NotifyPassenger(message, cabinetLink).Send();
					return true;
				}
				
			}
			catch (Exception ex)
			{
				_logger.ErrorException("User Controller GetNotifyPassenger", ex);
			}
			return false;
		}

		[OauthAuthorize]
		[Route("notifydriver")]
		public bool GetNotifyDriver([FromUri] int driver, [FromUri] int travel)
		{
			try
			{

				var tmpdriver = _userService.Get(driver);
				if (tmpdriver != null)
				{
					Message message = new Message();
					message.UserId = tmpdriver.Id;
					message.UserEmail = tmpdriver.Email;
					message.MessengerId = _auth.ThUser.Id;
					if (_auth != null && _auth.Token != null)
					{
						if (_auth.ThUser == null)
							_auth.ThUser = _userService.Get(_auth.User.Id.ToString(), _auth.Type);
					}
					message.MessengerDisplayName = _auth.ThUser.DisplayName;
					message.MessengerEmail = _auth.ThUser.Email;
					message.TravelId = travel;
					message.Theme = "Підписка на поїздку";
					message.MessageText = "На вашу поїздку підписався пасвжир.";
					message.State = false;
					message.Type = MessageType.Subscribe;
					_messageService.Add(message);
					string cabinetLink = string.Format("{0}://{1}/#/cabinet?message={2}",
						Request.RequestUri.Scheme, Request.RequestUri.Authority, message.Id);
					_mailer.NotifyDriver(message, cabinetLink).Send();
					return true;
				}
			}
			catch (Exception ex)
			{
				_logger.ErrorException("User Controller GetNotifyDriver", ex);
			}
			return false;
		}

		[MaybeAuthorize]
		[Route("friends")]
		public TravelFriends GetFriends([FromUri] int driver, [FromUri] int travel)
		{


			IEnumerable<MyFriend> friends = new List<MyFriend>();
			if (_auth != null && _auth.Token != null)
			{
				if (_auth.ThUser == null)
					_auth.ThUser = _userService.Get(_auth.User.Id.ToString(), _auth.Type);
				friends = _userService.GetFriends(_auth.ThUser.Id, driver);
			}
			return new TravelFriends
			{
				TravelId = travel,
				Friends = friends
			};
		}

		[OauthAuthorize]
		[Route("messages")]
		public IEnumerable<Message> GetMessages()
		{
			if (this._auth.ThUser == null && _auth.User != null)
				this._auth.ThUser = this._userService.Get(_auth.User.Id.ToString(), this._auth.Type);

			return _messageService.GetAll(this._auth.ThUser.Id);
		}

		[OauthAuthorize]
		[Route("message/{id:int}")]
		public Data.Message PutMessage(int id, Data.Message message)
		{
			message.Id = id;
			return _messageService.Save(message);
		}

		[OauthAuthorize]
		[Route("messagepost")]
		public Data.Message PostMessage(Data.Message message)
		{
			try
			{
				message.MessengerId = this._auth.ThUser.Id;
				message.MessengerEmail = this._auth.ThUser.Email;
				message.State = false;
				_mailer.SendMail(message).Send();
				return _messageService.Add(message);
			}
			catch (ArgumentException ex)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		[Route("callback")]
		public bool PostCallback(Data.Message message)
		{
			//try
			//{
			message.State = false;
			_mailer.SendCallBack(message).Send();
			return true;
			//}
			//catch (ArgumentException ex)
			//{
			//	throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
			//}
		}

		[OauthAuthorize]
		[Route("messages")]
		public void DeleteMessage([FromUri] IEnumerable<int> deletedIds)
		{
			foreach (var id in deletedIds)
			{
				_messageService.Delete(id);
			}

		}

		[OauthAuthorize]
		[Route("user")]
		public User GetCurentUser()
		{
			if (_auth != null && _auth.Token != null)
			{
				if (_auth.ThUser == null)
					_auth.ThUser = _userService.Get(_auth.User.Id.ToString(), _auth.Type);
			}
			return this._auth.ThUser;
		}
	}
}
