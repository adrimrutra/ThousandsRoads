var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var UserService = (function (_super) {
        __extends(UserService, _super);
        function UserService($resource) {
            _super.call(this, 'User', $resource);
            this.$superProvider = $resource('Api/User/:method/:id', { method: '@method', id: '@Id' }, {
                update: { method: 'PUT' }
            });
        }
        UserService.prototype.GetSocialid = function (token, success, error) {
            this.$superProvider.get({ id: token.SocialId, method: "socialid", type: token.Tokentype }, success, error);
        };
        UserService.prototype.PostComment = function (comment, success, error) {
            this.$superProvider.save({ method: "comment" }, comment, success, error);
        };
        UserService.prototype.NotifyPassenger = function (driver, travel, success, error) {
            this.$superProvider.get({ travel: travel, driver: driver, method: "notifypassenger" }, success, error);
        };
        UserService.prototype.NotifyDriver = function (driver, travel, success, error) {
            this.$superProvider.get({ travel: travel, driver: driver, method: "notifydriver" }, success, error);
        };
        UserService.prototype.GetFriends = function (driver, travel, success, error) {
            this.$superProvider.get({ travel: travel, driver: driver, method: "friends" }, success, error);
        };
        UserService.prototype.GetMessages = function (success, error) {
            this.$superProvider.query({ method: "messages" }, success, error);
        };
        UserService.prototype.DeleteMessages = function (deletedIds, success, error) {
            this.$superProvider.delete({ deletedIds: deletedIds, method: "messages" }, success, error);
        };
        UserService.prototype.PutMessage = function (message, success, error) {
            this.$superProvider.update({ id: message.Id, method: "message" }, message, success, error);
        };
        UserService.prototype.PostMessage = function (message, success, error) {
            this.$superProvider.save({ method: "messagepost" }, message, success, error);
        };
        UserService.prototype.PostCallBack = function (message, success, error) {
            this.$superProvider.save({ method: "callback" }, message, success, error);
        };
        UserService.prototype.GetCurenUser = function (success, error) {
            this.$superProvider.get({ method: "user" }, success, error);
        };
        return UserService;
    })(Application.Service);
    Application.UserService = UserService;
})(Application || (Application = {}));
//# sourceMappingURL=UserService.js.map