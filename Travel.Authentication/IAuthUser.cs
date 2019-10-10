using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel.Authentication
{
    public interface IAuthUser
    {
        long Id { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        Gender Gender { get; set; }
        string Token { get; set; }
        bool IsAuthorization { get; set; }
        IEnumerable<IAuthFriend> GetFriends();
        //string Avatar { get; set; }
    }

    public interface IAuthFriend
    {
        long Id { get; set; }
        string Name { get; set; }
    }

    public enum Gender
    { Male=1, Female=2, Other=3}
}
