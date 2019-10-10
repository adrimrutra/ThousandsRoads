/// <reference path="_references.ts" />
var Application;
(function (Application) {
    angular.module('Application', ['ng',
        'ngRoute',
        'ngResource',
        'AuthInterceptor',
        'ui.event',
        'ui.map',
        'ngAutocomplete',
        'ngCookies',
        'ui.draggable',
        'ui.bootstrap'
    ])
        .config(['$routeProvider', '$locationProvider', '$httpProvider', function ($routeProvider, $locationProvider, $httpProvider) {
            $httpProvider.defaults.withCredentials = true;
            $locationProvider.hashPrefix('');
            $routeProvider.when('/travel/view/:id', {
                Route: Application.Route.Travel,
                templateUrl: Application.TravelController.$viewtemplate,
                controller: Application.TravelController
            });
            $routeProvider.when('/travels', {
                /*################################################*/
                //helpTemplateUrl: TravelsController.$helptemplate,
                /*################################################*/
                Route: Application.Route.Travel,
                templateUrl: Application.TravelsController.$viewtemplate,
                controller: Application.TravelsController
            });
            $routeProvider.when('/travel/new', {
                Route: Application.Route.Travel,
                templateUrl: Application.NewTravelController.$viewtemplate,
                controller: Application.NewTravelController
            });
            $routeProvider.when('/travel/edit/:id', {
                Route: Application.Route.Travel,
                templateUrl: Application.NewTravelController.$viewtemplate,
                controller: Application.NewTravelController
            });
            $routeProvider.when('/user/new', {
                Route: Application.Route.Other,
                templateUrl: Application.NewUserController.$viewtemplate,
                controller: Application.NewUserController
            });
            $routeProvider.when('/home', {
                Route: Application.Route.Home,
                templateUrl: Application.HomeController.$viewtemplate,
                controller: Application.HomeController
            });
            $routeProvider.when('/search', {
                Route: Application.Route.Search,
                templateUrl: Application.SearchController.$viewtemplate,
                controller: Application.SearchController
            });
            $routeProvider.when('/cabinet', {
                Route: Application.Route.Other,
                templateUrl: Application.CabinetController.$viewtemplate,
                controller: Application.CabinetController
            });
            $routeProvider.when('/callback', {
                Route: Application.Route.CallBack,
                templateUrl: Application.CallBackController.$viewtemplate,
                controller: Application.CallBackController
            });
            $routeProvider.otherwise({ redirectTo: '/home' });
        }])
        .controller('TravelsController', Application.TravelsController)
        .controller('NewTravelController', Application.NewTravelController)
        .controller('ApplicationController', Application.ApplicationController)
        .controller('NewUserController', Application.NewUserController)
        .controller('AccountController', Application.AccountController)
        .controller('HomeController', Application.HomeController)
        .controller('SearchController', Application.SearchController)
        .controller('CabinetController', Application.CabinetController)
        .controller('CallBackController', Application.CallBackController)
        .directive('datetimepicker', function () {
        return {
            require: 'ngModel',
            scope: {
                model: "=ngModel",
                name: "@pickerName",
                isVisible: "@"
            },
            link: function (scope, $element, $attrs, ngModel) {
                $(function () {
                    $element.datetimepicker({
                        format: $attrs.format || 'yyyy-mm-dd hh:ii',
                        weekStart: 1,
                        autoclose: 1,
                        todayHighlight: 1,
                        startView: $attrs.startView || 'month',
                        endView: $attrs.endView || 'month',
                        todayBtn: $attrs.todayBtn ? parseInt($attrs.todayBtn) : parseInt("true"),
                        forceParse: 0,
                        showMeridian: 0,
                        minuteStep: 10,
                    }).on('changeDate', function (event) {
                        event.date.setHours(event.date.getHours() - (Math.round(-1 * event.date.getTimezoneOffset() / 60)));
                        ngModel.$setViewValue(event.date);
                    });
                    $element.on('show', function () {
                        $element.datetimepicker('update', ngModel.$viewValue);
                    });
                    $element.on('changeDate', function () {
                        scope.isVisible = false;
                    });
                    $element.on('$destroy', function () {
                        $element.data('datetimepicker').remove();
                    });
                    scope.$on("datetimepicker-show:" + scope.name, function () {
                        if (scope.isVisible)
                            $element.data('datetimepicker').hide();
                        else
                            $element.data('datetimepicker').show();
                        scope.isVisible = !scope.isVisible;
                    });
                });
            }
        };
    });
})(Application || (Application = {}));
//# sourceMappingURL=Application.js.map