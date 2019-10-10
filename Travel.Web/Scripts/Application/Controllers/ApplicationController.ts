/// <reference path="../_references.ts"/>

module Application {
    export interface IApplicationScope extends ng.IScope {
        GetDateString: Function;
        DefaultDateFormat: string;
        AccountTemplate: string;
        ScrollClass: Route;
        FacebookLogin: Function;
    }
    export enum Route {
        Other = 0, Home = 1, Forum = 2, Travel = 3, CallBack = 4, Search = 5
    }

    export class ApplicationController {
        constructor($scope: IApplicationScope, $rootScope: ng.IScope, $routeParams) {
            $scope.AccountTemplate = AccountController.$viewtemplate;
            $scope.DefaultDateFormat = 'Do MMMM  YYYY, h:mm:ss a';
            this.SetupScope($scope, $rootScope);
            this.SetupListener($scope, $rootScope);



        }
        SetupScope($scope: IApplicationScope, $rootScope: ng.IScope) {
            $scope.GetDateString = (date: Date, format: string) => {return moment(date).format(format) };
            $scope.FacebookLogin = () => {
                SocialAccount.Login('facebook');
            }

        }
        SetupListener($scope, $rootScope) {

            $rootScope.$on('$routeChangeSuccess', (event, current, pre) => {
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

        }
    }
    ApplicationController['$inject'] = ['$scope', '$rootScope', '$route'];
}