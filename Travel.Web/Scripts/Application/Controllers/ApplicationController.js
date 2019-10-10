/// <reference path="../_references.ts"/>
var Application;
(function (Application) {
    (function (Route) {
        Route[Route["Other"] = 0] = "Other";
        Route[Route["Home"] = 1] = "Home";
        Route[Route["Forum"] = 2] = "Forum";
        Route[Route["Travel"] = 3] = "Travel";
        Route[Route["CallBack"] = 4] = "CallBack";
        Route[Route["Search"] = 5] = "Search";
    })(Application.Route || (Application.Route = {}));
    var Route = Application.Route;
    var ApplicationController = (function () {
        function ApplicationController($scope, $rootScope, $routeParams) {
            $scope.AccountTemplate = Application.AccountController.$viewtemplate;
            $scope.DefaultDateFormat = 'Do MMMM  YYYY, h:mm:ss a';
            this.SetupScope($scope, $rootScope);
            this.SetupListener($scope, $rootScope);
        }
        ApplicationController.prototype.SetupScope = function ($scope, $rootScope) {
            $scope.GetDateString = function (date, format) { return moment(date).format(format); };
            $scope.FacebookLogin = function () {
                Application.SocialAccount.Login('facebook');
            };
        };
        ApplicationController.prototype.SetupListener = function ($scope, $rootScope) {
            $rootScope.$on('$routeChangeSuccess', function (event, current, pre) {
                if (current.$$route)
                    $scope.ScrollClass = current.$$route.Route;
            });
            //$(window).scroll(() => {
            //    var helpPos = $('#help-floating').offset().top;
            //    var road = $('.road').offset().top;
            //    var helpHeight = $('#help-floating').outerHeight();
            //    if (helpPos > road - helpHeight) {
            //        $('#help-floating').css({ 'position': 'absolute', 'top': road - helpHeight });
            //    }
            //    else {
            //        $('#help-floating').css({ 'position': 'fixed', 'top': '90%' });
            //    }
            //});
        };
        return ApplicationController;
    })();
    Application.ApplicationController = ApplicationController;
    ApplicationController['$inject'] = ['$scope', '$rootScope', '$route'];
})(Application || (Application = {}));
//# sourceMappingURL=ApplicationController.js.map