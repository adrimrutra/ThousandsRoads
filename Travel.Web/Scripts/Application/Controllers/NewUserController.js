/// <reference path="../_references.ts"/>
var Application;
(function (Application) {
    var NewUserController = (function () {
        function NewUserController($scope, $rootScope) {
            this.SetupScope($scope, $rootScope);
        }
        NewUserController.prototype.SetupScope = function ($scope, $rootScope) {
        };
        NewUserController.$viewtemplate = "/Content/Templates/NewUser.html";
        return NewUserController;
    })();
    Application.NewUserController = NewUserController;
    NewUserController['$inject'] = ['$scope', '$rootScope'];
})(Application || (Application = {}));
//# sourceMappingURL=NewUserController.js.map