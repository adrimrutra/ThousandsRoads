/// <reference path="_references.ts" />
module Application {
    angular.module('Application',
        ['ng',
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
        .config(['$routeProvider', '$locationProvider', '$httpProvider', ($routeProvider: ng.route.IRouteProvider, $locationProvider: ng.ILocationProvider, $httpProvider: ng.IHttpProvider)=> {
            $httpProvider.defaults.withCredentials = true;

            $locationProvider.hashPrefix('');

            $routeProvider.when('/travel/view/:id',
                {
                    Route: Route.Travel,
                    templateUrl: TravelController.$viewtemplate,
                    controller: TravelController
                });
            $routeProvider.when('/travels',
                {
                    /*################################################*/
                    //helpTemplateUrl: TravelsController.$helptemplate,
                    /*################################################*/
                    Route: Route.Travel,
                    templateUrl: TravelsController.$viewtemplate,
                    controller: TravelsController
                });
            $routeProvider.when('/travel/new',
                {
                    Route: Route.Travel,
                    templateUrl: NewTravelController.$viewtemplate,
                    controller: NewTravelController
                });
            $routeProvider.when('/travel/edit/:id',
                {
                    Route: Route.Travel,
                    templateUrl: NewTravelController.$viewtemplate,
                    controller: NewTravelController
                });
            $routeProvider.when('/user/new',
                {
                    Route: Route.Other,
                    templateUrl: NewUserController.$viewtemplate,
                    controller: NewUserController
                });
            $routeProvider.when('/home',
                {
                    Route: Route.Home,
                    templateUrl: HomeController.$viewtemplate,
                    controller: HomeController
                });
            $routeProvider.when('/search',
                {
                    Route: Route.Search,
                    templateUrl: SearchController.$viewtemplate,
                    controller: SearchController
                });
            $routeProvider.when('/cabinet',
                {
                    Route: Route.Other,
                    templateUrl: CabinetController.$viewtemplate,
                    controller: CabinetController
                });
            $routeProvider.when('/callback',
                {
                    Route: Route.CallBack,
                    templateUrl: CallBackController.$viewtemplate,
                    controller: CallBackController
                });

            $routeProvider.otherwise({ redirectTo: '/home' });
        }])
        .controller('TravelsController', TravelsController)
        .controller('NewTravelController', NewTravelController)
        .controller('ApplicationController', ApplicationController)
        .controller('NewUserController', NewUserController)
        .controller('AccountController', AccountController)
        .controller('HomeController', HomeController)
        .controller('SearchController', SearchController)
        .controller('CabinetController', CabinetController)
        .controller('CallBackController', CallBackController)
        .directive('datetimepicker', () => {
            return {
                require: 'ngModel',
                scope: {
                    model: "=ngModel",
                    name: "@pickerName",
                    isVisible:"@"
                },
                link: (scope, $element, $attrs, ngModel) => {
                    $(() => {
                        $element.datetimepicker({
                            format: $attrs.format || 'yyyy-mm-dd hh:ii',
                            weekStart: 1,
                            autoclose: 1,
                            todayHighlight: 1,
                            startView: $attrs.startView || 'month',
                            endView: $attrs.endView || 'month',
                            todayBtn: $attrs.todayBtn ? parseInt($attrs.todayBtn) : parseInt("true") ,
                            forceParse: 0,
                            showMeridian: 0,
                            minuteStep: 10,
                            
                        }).on('changeDate', function (event) {
                                event.date.setHours(event.date.getHours() - (Math.round(-1 * event.date.getTimezoneOffset() / 60)));
                                ngModel.$setViewValue(event.date);
                            });
                        $element.on('show', () => {
                            $element.datetimepicker('update', ngModel.$viewValue);
                        });
                        $element.on('changeDate', () => {
                            scope.isVisible = false;
                        });
                        $element.on('$destroy', () => {
                            $element.data('datetimepicker').remove();
                        });
                        scope.$on("datetimepicker-show:" + scope.name, () => {
                            if (scope.isVisible)
                                $element.data('datetimepicker').hide();
                            else
                                $element.data('datetimepicker').show();
                            scope.isVisible = !scope.isVisible;
                        });
                    });
                }
            };
        })
    ;
}