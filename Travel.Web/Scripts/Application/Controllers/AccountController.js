/// <reference path="../_references.ts"/>
var Application;
(function (Application) {
    var AccountController = (function () {
        function AccountController($scope, $rootScope, $resource, $cookieStore, $window) {
            var _this = this;
            if (this.$userService == null)
                this.$userService = new Application.UserService($resource);
            $scope.RegistrationForm = $('#registration').modal('hide');
            Application.SocialAccount.Create($window.OAuth, $cookieStore);
            this.$socialAccount = Application.SocialAccount.GetAccount();
            if (this.$socialAccount != null) {
                $scope.User = new Application.User();
                this.$socialAccount.GetInfo().done(function (success) {
                    $scope.UserInfo = success;
                    $scope.Token = new Application.Token();
                    $scope.Token.SocialId = success.id;
                    $scope.Token.Tokentype = Application.TokenType.facebook;
                    _this.$userService.GetSocialid($scope.Token, function (model) { $scope.FindUser(model); }, function (error) { console.log(error); });
                });
                $(document).ready(function () {
                    $("[data-toggle=tooltip]").tooltip({ placement: 'right' });
                });
                this.$socialAccount.GetAvatar().done(function (avatar) {
                    $scope.User.Avatar = avatar.data.url;
                });
            }
            this.SetupScope($scope, $rootScope, $cookieStore, $window);
        }
        AccountController.prototype.SetupScope = function ($scope, $rootScope, $cookieStore, $window) {
            var _this = this;
            $scope.FacebookLogin = function () {
                Application.SocialAccount.Login('facebook');
            };
            $scope.VkontakteLogin = function () {
            };
            $scope.OdnoklassnikiLogin = function () {
            };
            ////////////////////////////////////////////////////////////////
            $scope.FindUser = function (model) {
                if (model.Id == undefined || model.Id == 0) {
                    $scope.RegistrationForm.modal('show');
                }
                $scope.User.Id = model.Id;
                $scope.User.DisplayName = model.DisplayName;
                $scope.User.Email = model.Email;
            };
            $scope.Exit = function () {
                $scope.User.Id = null;
                $scope.User = null;
                $scope.UserInfo = null;
                Application.SocialAccount.ClearCache();
                $rootScope.$broadcast("event:logout");
            };
            $scope.Registration = function () {
                $scope.User.DisplayName = $scope.UserInfo.name;
                $scope.User.Email = $scope.UserInfo.email;
                $scope.User.Tokens = new Array();
                $scope.User.Tokens.push($scope.Token);
                $scope.User.Id = null;
                _this.$userService.Create($scope.User, function (model) {
                    $scope.RegistrationForm.modal('hide');
                    $scope.User = model;
                }, function (error) { console.log(error); });
            };
            $scope.$watch("User.Id", function () {
                if ($scope.User)
                    AccountController.UserId = $scope.User.Id;
                $rootScope.$broadcast("event:logining");
            });
        };
        AccountController.$viewtemplate = "/Content/Templates/Account.html";
        return AccountController;
    })();
    Application.AccountController = AccountController;
    AccountController['$inject'] = ['$scope', '$rootScope', '$resource', '$cookieStore', '$window'];
})(Application || (Application = {}));
//# sourceMappingURL=AccountController.js.map