using System;
using Travel.Common.Enums;
using Travel.Data;

namespace Travel.Authentication
{
	public class Authentification : IAuthentification
	{
		private IAuthUser user;
		public IAuthUser User
		{
			get
			{
				if (user == null && Create != null)
					user = Create(Token);
				return user;
			}
			set
			{
				user = value;
			}
		}
		public Func<string, IAuthUser> Create { get; set; }

		public string Token { get; set; }

		public User ThUser { get; set; }

		public TokenType Type { get; set; }
	}
	public interface IAuthentification
	{
		IAuthUser User { get; set; }
		User ThUser { get; set; }
		string Token { get; set; }
		Func<string, IAuthUser> Create { get; set; }
		TokenType Type { get; set; }
	}
}
