/// <reference path="../_references.ts" />
module Application {
    
    export interface IHomeScope extends ng.IScope {
       
        // GetDateString: Function;
    }
    export class HomeController {
        public static $viewtemplate = "/Content/Templates/Home.html";
        constructor($scope: IHomeScope) {

        }
    }
    HomeController['$inject'] = ['$scope'];
} 