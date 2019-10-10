using System.Collections.Generic;
using Travel.Authentication;
using Travel.Common.Enums;
using Travel.Data;

namespace Travel.Service.Services.Abstractions
{
	public interface IUserService : IService<User>
	{
		User Get(string SocialId, TokenType token);
		void UpdateFriedsList(IAuthentification auth);
		IEnumerable<MyFriend> GetFriends(int userId, int driverId);
	}
}
