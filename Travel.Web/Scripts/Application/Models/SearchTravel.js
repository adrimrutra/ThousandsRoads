/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var SearchTravel = (function () {
        function SearchTravel() {
        }
        return SearchTravel;
    })();
    Application.SearchTravel = SearchTravel;
    (function (WaitType) {
        WaitType[WaitType["Default"] = 0] = "Default";
        WaitType[WaitType["Five"] = 1] = "Five";
        WaitType[WaitType["Ten"] = 2] = "Ten";
        WaitType[WaitType["Fifteen"] = 3] = "Fifteen";
        WaitType[WaitType["Twenty"] = 4] = "Twenty";
    })(Application.WaitType || (Application.WaitType = {}));
    var WaitType = Application.WaitType;
    (function (EarlyType) {
        EarlyType[EarlyType["Default"] = 0] = "Default";
        EarlyType[EarlyType["Five"] = 1] = "Five";
        EarlyType[EarlyType["Ten"] = 2] = "Ten";
        EarlyType[EarlyType["Fifteen"] = 3] = "Fifteen";
        EarlyType[EarlyType["Twenty"] = 4] = "Twenty";
    })(Application.EarlyType || (Application.EarlyType = {}));
    var EarlyType = Application.EarlyType;
})(Application || (Application = {}));
//# sourceMappingURL=SearchTravel.js.map