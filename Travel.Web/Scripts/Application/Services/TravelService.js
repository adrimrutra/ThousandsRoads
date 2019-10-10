var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var TravelService = (function (_super) {
        __extends(TravelService, _super);
        function TravelService($resource) {
            _super.call(this, 'Travel', $resource);
            this.$superProvider = $resource('Api/Travel/:method/:id', { method: '@method', id: '@Id' }, {
                update: { method: 'PUT' }
            });
        }
        TravelService.prototype.PutTraveler = function (TravelId, UserId, success, error) {
            this.$superProvider.get({ TravelId: TravelId, UserId: UserId, method: "traveler" }, success, error);
        };
        return TravelService;
    })(Application.Service);
    Application.TravelService = TravelService;
})(Application || (Application = {}));
//# sourceMappingURL=TravelService.js.map