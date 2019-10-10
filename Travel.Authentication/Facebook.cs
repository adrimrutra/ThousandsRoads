using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FB = ComputerBeacon.Facebook.Graph;

namespace Travel.Authentication
{
    public class Facebook : IAuthUser
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public Gender Gender { get; set; }

        public string Token { get; set; }

        public bool IsAuthorization { get; set; }

        private IEnumerable<IAuthFriend> friend;
        public IEnumerable<IAuthFriend> GetFriends()
        {
            if (friend == null)
            {
                var friends = FB.User.GetFriends(Id.ToString(), Token);
                friend = friends.Select(x => new FacebookFriend { Id = long.Parse(x.Id), Name = x.Name }).ToList();
            }
            return friend;
        }
    }

    public class FacebookFriend : IAuthFriend
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }

    public class FacebookFactory
    {
        public static IAuthUser Create(string token)
        {
            var user = new FB.User("me", token);
            if (user == null)
                return null;
            Gender gender;
            switch (user.Gender)
            {
                case FB.Gender.Female:
                    gender = Gender.Female;
                    break;
                case FB.Gender.Male:
                    gender = Gender.Male;
                    break;
                default:
                    gender = Gender.Other;
                    break;
            }

            return new Facebook
            {
                Email = user.Email,
                Id = long.Parse(user.Id),
                Name = user.Name,
                Token = token,
                Gender = gender
            };
        }
    }
}
