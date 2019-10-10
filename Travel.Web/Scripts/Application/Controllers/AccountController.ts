
/// <reference path="../_references.ts"/>
module Application {
    export interface IAccountScope extends ng.IScope {
        User: User;
        Token: Token;
        FacebookLogin: Function;
        VkontakteLogin: Function;
        OdnoklassnikiLogin: Function;
        FindUser: Function;
        Exit: Function;
        UserInfo: any;
        RegistrationForm: any;
        Registration: Function;
        SetAuthHeader: Function;
        Close: Function;
    }

    export class AccountController {
        public static $viewtemplate = "/Content/Templates/Account.html";
        private $userService: UserService;
        private $socialAccount: ISocialScope;
        public static UserId: number;
        constructor($scope: IAccountScope, $rootScope: ng.IScope, $resource: IResourceService, $cookieStore: ng.cookies.ICookieStoreService, $window) {
            if (this.$userService == null)
                this.$userService = new UserService($resource);

            $scope.RegistrationForm = $('#registration').modal('hide');

            SocialAccount.Create($window.OAuth, $cookieStore);
            this.$socialAccount = SocialAccount.GetAccount();
            if (this.$socialAccount != null) {
                $scope.User = new User();
                this.$socialAccount.GetInfo().done((success) => {
                    $scope.UserInfo = success;
                    $scope.Token = new Token();
                    $scope.Token.SocialId = success.id;
                    $scope.Token.Tokentype = TokenType.facebook;
                    this.$userService.GetSocialid($scope.Token,
                        (model: User) => { $scope.FindUser(model); },
                        (error) => { console.log(error) });
                });

                $(document).ready(function () {
                    $("[data-toggle=tooltip]").tooltip({ placement: 'right' });
                });

                this.$socialAccount.GetAvatar().done((avatar) => {
                    $scope.User.Avatar = avatar.data.url;
                });

            }

            this.SetupScope($scope, $rootScope, $cookieStore, $window);
        }
        SetupScope($scope: IAccountScope, $rootScope: ng.IScope, $cookieStore: ng.cookies.ICookieStoreService, $window) {
            $scope.FacebookLogin = () => {
                
                SocialAccount.Login('facebook');
            }
            $scope.VkontakteLogin = () => {
               
            }
            $scope.OdnoklassnikiLogin = () => {
               
            }
              ////////////////////////////////////////////////////////////////
            $scope.FindUser = (model: User) => {
                if (model.Id == undefined || model.Id == 0) {
                    $scope.RegistrationForm.modal('show');
                }
                
                $scope.User.Id = model.Id;
                $scope.User.DisplayName = model.DisplayName;
                $scope.User.Email = model.Email;
            }
       

            $scope.Exit = () => {
                $scope.User.Id = null;
                $scope.User = null;
                $scope.UserInfo = null;
                SocialAccount.ClearCache();
                $rootScope.$broadcast("event:logout");
            }
          
         
            $scope.Registration = () => {
                $scope.User.DisplayName = $scope.UserInfo.name;
                $scope.User.Email = $scope.UserInfo.email;
                $scope.User.Tokens = new Array<Token>();
                $scope.User.Tokens.push($scope.Token);
                $scope.User.Id = null;
                this.$userService.Create($scope.User,
                    (model: User) => {
                        $scope.RegistrationForm.modal('hide');
                        $scope.User = model;
                    },
                    (error) => { console.log(error) });
            }


            $scope.$watch("User.Id", () => {
                if ($scope.User)
                    AccountController.UserId = $scope.User.Id;
                $rootScope.$broadcast("event:logining");
            });
        }
    }
    AccountController['$inject'] = ['$scope', '$rootScope', '$resource', '$cookieStore', '$window'];
}