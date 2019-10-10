using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel.Authentication
{
    public class Fake : IAuthUser
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
            var tmpfriend = new List<IAuthFriend>();
            var friends = Token.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Skip(1);
            foreach (var f in friends)
            {
                var tmp = f.Split(new char[] { ':' });
                tmpfriend.Add(new FakeFriend { Id = long.Parse(tmp.First()), Name = tmp.Last() });
            }
            return tmpfriend;
        }
    }

    public class FakeFriend : IAuthFriend
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }

    public class FakeFactory
    {
        public static IAuthUser Create(string token)
        {
            return new Fake
            {
                Token = token,
                Name = token.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).First()
            };
        }
    }
}
