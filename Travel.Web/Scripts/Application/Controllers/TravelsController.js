/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var TravelsController = (function () {
        function TravelsController($scope, $rootScope, $resource, $location) {
            var _this = this;
            $scope.CurentUserId = Application.AccountController.UserId;
            if (this.$travelService == null)
                this.$travelService = new Application.TravelService($resource);
            if (this.$userService == null)
                this.$userService = new Application.UserService($resource);
            //if ($scope.AllTravelsFriends)
            //    {
            //    console.log("VUDILUV======>");
            //        $scope.AllTravelsFriends = new Array<TravelFriends>();
            //}
            this.$travelService.GetAll({}, function (modules) {
                $scope.Travels = modules;
                $scope.Travels.forEach(function (travel) {
                    _this.$userService.GetFriends(travel.DriverId, travel.Id, function (model) {
                        travel.Friends = model.Friends;
                    }, function (error) { console.log(error); });
                });
            }, function (error) { return console.log(error); });
            $scope.DisplayName = function (value) {
                if (value) {
                    var values = value.split(' ');
                    return values[0][0] + ". " + values[1];
                }
            };
            this.SetupScope($scope, $rootScope, $location);
        }
        TravelsController.prototype.SetupScope = function ($scope, $rootScope, $location) {
            var _this = this;
            $scope.AddTravel = function () { $location.path("/travel/new"); };
            $scope.EditTravel = function (Id) { $location.path("/travel/edit/" + Id.toString()); };
            $scope.DeleteTravel = function (model) {
                _this.$travelService.Delete(model, function () {
                    var index = $scope.Travels.map(function (value) { return value.Id; }).indexOf(model.Id);
                    $scope.Travels.splice(index, 1);
                }, function (error) { console.log(error); });
            };
            $scope.GetNameMappoint = function (name) {
                if (name)
                    return name.split(",");
            };
            $scope.ViewTravel = function (Id) { $location.path("/travel/view/" + Id.toString()); };
            $scope.GetDateString = function (date) { return moment(date).format('DD MM  YYYY'); };
            $rootScope.$on("event:logining", function (data) {
                var args = [];
                for (var _i = 1; _i < arguments.length; _i++) {
                    args[_i - 1] = arguments[_i];
                }
                $scope.CurentUserId = Application.AccountController.UserId;
            });
            $rootScope.$on("event:logout", function (data) {
                var args = [];
                for (var _i = 1; _i < arguments.length; _i++) {
                    args[_i - 1] = arguments[_i];
                }
                $scope.CurentUserId = undefined;
            });
        };
        TravelsController.$viewtemplate = "/Content/Templates/Travels.html";
        TravelsController.$helptemplate = "/Content/Templates/TravelsHelp.html";
        return TravelsController;
    })();
    Application.TravelsController = TravelsController;
    TravelsController['$inject'] = ['$scope', '$rootScope', '$resource', '$location'];
})(Application || (Application = {}));
//# sourceMappingURL=TravelsController.js.map