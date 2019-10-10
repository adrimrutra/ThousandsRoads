using Mvc.Mailer;
using System.Collections.Generic;
using Travel.Authentication;
using Travel.Core;
using System.Web.Mvc;
using System.Web;
using System;
using System.IO;

namespace Travel.Web.Mail
{
    public class Mailer : MailerBase, IMailer
    {
        private IAuthentification auth;
        public Mailer(IAuthentification _auth)
        {
            MasterName = "_Layout";
            this.auth = _auth;
        }
        public MvcMailMessage Welcome(Data.User model)
        {
            //ViewBag.Data = someObject;
            return Populate(x =>
            {
                x.Subject = "Welcome";
                x.ViewName = "Welcome";
                x.To.Add(model.Email);
            });
        }
	
		public MvcMailMessage NotifyDriver(Data.Message message, string cabinetLink)
		{
			var resources = new Dictionary<string, string>();
			var logo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
				"Content/Images/logo.png");
			var road = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
			   "Content/Images/road-mail.png");
			resources["logo"] = logo;
			resources["road"] = road;

			ViewBag.CabinetLink = cabinetLink;
			ViewBag.MessageText = "Від  " + message.MessengerDisplayName + ".      " + message.MessageText + "  " + message.MessengerEmail;
			return Populate(x =>
			{
				x.LinkedResources = resources;
				x.Subject = message.Theme;
				x.ViewName = "Base";
				x.To.Add(message.UserEmail);
			});

		}
		public MvcMailMessage NotifyPassenger(Data.Message message, string cabinetLink)
		{
			var resources = new Dictionary<string, string>();
			var logo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
				"Content/Images/logo.png");
			var road = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
			   "Content/Images/road-mail.png");
			resources["logo"] = logo;
			resources["road"] = road;

			ViewBag.User = this.auth.ThUser;
			ViewBag.MessageText = "Від  " + message.MessengerDisplayName + ".      " + message.MessageText + "  " + message.MessengerEmail;
			return Populate(x =>
			{
				x.LinkedResources = resources;
				x.Subject = message.Theme;
				x.ViewName = "Base";
				x.To.Add(message.UserEmail);
			});
		}

        public MvcMailMessage SendMail(Data.Message message)
        {

			var resources = new Dictionary<string, string>();
			var logo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
				"Content/Images/logo.png");
			var road = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
			   "Content/Images/road-mail.png");
			resources["logo"] = logo;
			resources["road"] = road;


			//ViewBag.User = this.auth.ThUser;
			ViewBag.MessageText = "Від  " + message.MessengerDisplayName + ".      " + message.MessageText + "  " + message.MessengerEmail;
            ViewBag.MessageText = message.MessageText;
            return Populate(x =>
            {
				x.LinkedResources = resources;
                x.Subject = message.Theme;
                x.ViewName = "Base";
                x.To.Add(message.UserEmail);
            });
        }
        public MvcMailMessage SendCallBack(Data.Message message)
        {
            var resources = new Dictionary<string, string>();
            var logo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Content/Images/logo.png");
			var	road = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
			   "Content/Images/road-mail.png");
			resources["logo"] = logo;
			resources["road"] = road;
			ViewBag.MessageText = "Від  " + message.MessengerDisplayName + ".      " + message.MessageText + "  " + message.MessengerEmail;
            return Populate(x =>
            {
                x.LinkedResources = resources;
                x.Subject = message.Theme;
                x.ViewName = "Base";
                x.To.Add(message.UserEmail);
            });
        }




		
	}
}