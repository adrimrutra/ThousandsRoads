/// <reference path="../_references.ts"/>
module Application {
    export interface INewUserScope extends ng.IScope {
        User: User;
    }

    export class NewUserController {
        public static $viewtemplate = "/Content/Templates/NewUser.html";
        constructor($scope: INewUserScope, $rootScope: ng.IScope) {
          
            this.SetupScope($scope, $rootScope);

        }
        SetupScope($scope: INewUserScope, $rootScope: ng.IScope) {
            
        }
    }
    NewUserController['$inject'] = ['$scope', '$rootScope'];
}