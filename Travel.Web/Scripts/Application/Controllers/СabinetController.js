/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var CabinetController = (function () {
        function CabinetController($scope) {
        }
        CabinetController.$viewtemplate = "/Content/Templates/Cabinet.html";
        return CabinetController;
    })();
    Application.CabinetController = CabinetController;
    CabinetController['$inject'] = ['$scope'];
})(Application || (Application = {}));
//# sourceMappingURL=СabinetController.js.map
