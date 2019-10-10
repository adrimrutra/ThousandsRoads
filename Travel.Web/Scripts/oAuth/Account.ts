/// <reference path="../Application/_references.ts"/>

module Application {
    export interface ISocialScope {
        GetInfo: Function;
        GetFriends: Function;
        InviteFriend: Function;
        GetAvatar: Function;
        GetCustom: Function;
        OAuth: any;
    }

    export class SocialAccount {
        private static oAuth: any;
        private static Social: ISocialScope;
        private static $cookieStore: ng.cookies.ICookieStoreService;

     public static accessTokenString = 'access_token';
        //   public static accessTokenString = '3a3f933cce529c4d486e7de070c28657';

        
        public static accessTokenTypeString = 'access_token_type';
     
        public static Create = (oAuth: any, $cookieStore: ng.cookies.ICookieStoreService) => {
            SocialAccount.oAuth = oAuth;
            SocialAccount.$cookieStore = $cookieStore;
          //  SocialAccount.oAuth.initialize('eK6IikYs1JmF42Vn_bg6BbQ_PVE', { cache: false });
            SocialAccount.oAuth.initialize('3a3f933cce529c4d486e7de070c28657', { cache: false }); 
            
                      
            SocialAccount.FacebookCalllBack();
        }
        public static GetAccount = () => {
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
        }
        public static Login = (poviderName: string) => {
            SocialAccount.oAuth.redirect(poviderName, { cache: false }, '/');
        }
        public static ClearCache = () => {
            SocialAccount.oAuth.clearCache();
            SocialAccount.$cookieStore.remove(SocialAccount.accessTokenTypeString);
            SocialAccount.$cookieStore.remove(SocialAccount.accessTokenString);
            angular.element("#application").scope().$broadcast('event:auth-logout');
            SocialAccount.Social = null;
        }

      
     
         private static FacebookCalllBack = () => {
            SocialAccount.oAuth.callback('facebook', (error, success) => {
                SocialAccount.Social = new Facebook(SocialAccount.oAuth, success.access_token);
                SocialAccount.$cookieStore.put(SocialAccount.accessTokenTypeString, 'facebook');
                SocialAccount.$cookieStore.put(SocialAccount.accessTokenString, success.access_token);
            });
        }
    }
    export class AuthHeader {
        scheme: string;
        param: string;
    }
    export enum AuthScheme {
        fb
    }
    export class SocialFriends {
        data: Array<SocialFriend>;
        paging: SocialPaging;
    }
    export class SocialFriend {
        id: number;
        name: string;
    }
    export class SocialPaging {
        next: string;
        previous: string;
    }
    export class Facebook implements ISocialScope {
        private $providerName = 'facebook';
        public static ProfilAlbum = "Profile Pictures";
        public OAuth: any;
        constructor(oAuth: any, accesse: string) {
            this.OAuth = oAuth.create(this.$providerName,
                {
                    oauth_token: accesse,
                },
                {
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

        GetInfo = () => { return this.OAuth.get("/me"); }
        GetFriends = () => { return this.OAuth.get("me/friends?limit=10&&Offset=0"); }
        InviteFriend = (id: number) => {
            return this.OAuth.post("/me/notifications","POST",
                                                {
                                                    "object": {
                                                        "href": "\/testurl?param1=value1",
                                                        "template": "This is a test message"
                                                    }
                                                }); }
        GetCustom = (query: string) => { return this.OAuth.get(query); }
        GetAvatar = () => { return this.OAuth.get("/me/picture?redirect=0&type=large"); }

    }
}
