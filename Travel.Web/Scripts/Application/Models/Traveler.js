/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var Traveler = (function () {
        function Traveler() {
        }
        return Traveler;
    })();
    Application.Traveler = Traveler;
    (function (UserType) {
        UserType[UserType["Driver"] = 1] = "Driver";
        UserType[UserType["Passenger"] = 2] = "Passenger";
    })(Application.UserType || (Application.UserType = {}));
    var UserType = Application.UserType;
})(Application || (Application = {}));
//# sourceMappingURL=Traveler.js.map