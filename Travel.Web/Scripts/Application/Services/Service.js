/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var Service = (function () {
        function Service($controller, $resource) {
            this.$controller = $controller;
            this.$resource = $resource;
            this.$provider = this.$resource('Api/:controller/:id', { controller: $controller, id: '@Id' }, {
                update: { method: 'PUT' }
            });
        }
        Service.prototype.Get = function (id, success, error) {
            this.$provider.get({ id: id }, success, error);
        };
        Service.prototype.GetAll = function (filter, success, error) {
            this.$provider.query(filter, success, error);
        };
        Service.prototype.Create = function (model, success, error) {
            this.$provider.save(model, success, error);
        };
        Service.prototype.Delete = function (model, success, error) {
            this.$provider.delete(null, model, success, error);
        };
        Service.prototype.Update = function (model, success, error) {
            this.$provider.update(model, success, error);
        };
        return Service;
    })();
    Application.Service = Service;
})(Application || (Application = {}));
//# sourceMappingURL=Service.js.map