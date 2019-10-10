var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var TravelerService = (function (_super) {
        __extends(TravelerService, _super);
        function TravelerService($resource) {
            _super.call(this, 'Traveler', $resource);
        }
        return TravelerService;
    })(Application.Service);
    Application.TravelerService = TravelerService;
})(Application || (Application = {}));
//# sourceMappingURL=TravelerService.js.map