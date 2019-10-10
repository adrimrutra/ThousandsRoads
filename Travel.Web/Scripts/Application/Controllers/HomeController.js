/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var HomeController = (function () {
        function HomeController($scope) {
        }
        HomeController.$viewtemplate = "/Content/Templates/Home.html";
        return HomeController;
    })();
    Application.HomeController = HomeController;
    HomeController['$inject'] = ['$scope'];
})(Application || (Application = {}));
//# sourceMappingURL=HomeController.js.map