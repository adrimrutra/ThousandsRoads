/// <reference path="../Application/_references.ts"/>
var Application;
(function (Application) {
    var SocialAccount = (function () {
        function SocialAccount() {
        }
        SocialAccount.accessTokenString = 'access_token';
        //   public static accessTokenString = '3a3f933cce529c4d486e7de070c28657';
        SocialAccount.accessTokenTypeString = 'access_token_type';
        SocialAccount.Create = function (oAuth, $cookieStore) {
            SocialAccount.oAuth = oAuth;
            SocialAccount.$cookieStore = $cookieStore;
            //  SocialAccount.oAuth.initialize('eK6IikYs1JmF42Vn_bg6BbQ_PVE', { cache: false });
            SocialAccount.oAuth.initialize('3a3f933cce529c4d486e7de070c28657', { cache: false });
            SocialAccount.FacebookCalllBack();
        };
        SocialAccount.GetAccount = function () {
            if (SocialAccount.Social != null) {
                return SocialAccount.Social;
            }
            var providerName = SocialAccount.$cookieStore.get(SocialAccount.accessTokenTypeString);
            switch (providerName) {
                case 'facebook':
                    {
                        var accesse = SocialAccount.$cookieStore.get(SocialAccount.accessTokenString);
                        SocialAccount.Social = new Facebook(SocialAccount.oAuth, accesse);
                        return SocialAccount.Social;
                    }
                default:
                    return undefined;
            }
        };
        SocialAccount.Login = function (poviderName) {
            SocialAccount.oAuth.redirect(poviderName, { cache: false }, '/');
        };
        SocialAccount.ClearCache = function () {
            SocialAccount.oAuth.clearCache();
            SocialAccount.$cookieStore.remove(SocialAccount.accessTokenTypeString);
            SocialAccount.$cookieStore.remove(SocialAccount.accessTokenString);
            angular.element("#application").scope().$broadcast('event:auth-logout');
            SocialAccount.Social = null;
        };
        SocialAccount.FacebookCalllBack = function () {
            SocialAccount.oAuth.callback('facebook', function (error, success) {
                SocialAccount.Social = new Facebook(SocialAccount.oAuth, success.access_token);
                SocialAccount.$cookieStore.put(SocialAccount.accessTokenTypeString, 'facebook');
                SocialAccount.$cookieStore.put(SocialAccount.accessTokenString, success.access_token);
            });
        };
        return SocialAccount;
    })();
    Application.SocialAccount = SocialAccount;
    var AuthHeader = (function () {
        function AuthHeader() {
        }
        return AuthHeader;
    })();
    Application.AuthHeader = AuthHeader;
    (function (AuthScheme) {
        AuthScheme[AuthScheme["fb"] = 0] = "fb";
    })(Application.AuthScheme || (Application.AuthScheme = {}));
    var AuthScheme = Application.AuthScheme;
    var SocialFriends = (function () {
        function SocialFriends() {
        }
        return SocialFriends;
    })();
    Application.SocialFriends = SocialFriends;
    var SocialFriend = (function () {
        function SocialFriend() {
        }
        return SocialFriend;
    })();
    Application.SocialFriend = SocialFriend;
    var SocialPaging = (function () {
        function SocialPaging() {
        }
        return SocialPaging;
    })();
    Application.SocialPaging = SocialPaging;
    var Facebook = (function () {
        function Facebook(oAuth, accesse) {
            var _this = this;
            this.$providerName = 'facebook';
            this.GetInfo = function () { return _this.OAuth.get("/me"); };
            this.GetFriends = function () { return _this.OAuth.get("me/friends?limit=10&&Offset=0"); };
            this.InviteFriend = function (id) {
                return _this.OAuth.post("/me/notifications", "POST", {
                    "object": {
                        "href": "\/testurl?param1=value1",
                        "template": "This is a test message"
                    }
                });
            };
            this.GetCustom = function (query) { return _this.OAuth.get(query); };
            this.GetAvatar = function () { return _this.OAuth.get("/me/picture?redirect=0&type=large"); };
            this.OAuth = oAuth.create(this.$providerName, {
                oauth_token: accesse,
            }, {
                "url": "https://graph.facebook.com",
                "cors": true,
                "query": {
                    "access_token": accesse
                }
            });
            var headerInfo = new AuthHeader();
            headerInfo.scheme = 'fb';
            headerInfo.param = accesse;
            angular.element("#application").scope().$broadcast('event:auth-login', headerInfo);
        }
        Facebook.ProfilAlbum = "Profile Pictures";
        return Facebook;
    })();
    Application.Facebook = Facebook;
})(Application || (Application = {}));
//# sourceMappingURL=Account.js.map