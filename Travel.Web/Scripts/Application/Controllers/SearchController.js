/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var SearchController = (function () {
        function SearchController($scope, $rootScope, $routeParams, $resource, $location) {
            var _this = this;
            //$location: ng.ILocationService
            //$routeParams: IRouteParams
            if (this.$travelService == null)
                this.$travelService = new Application.TravelService($resource);
            if (this.$userService == null)
                this.$userService = new Application.UserService($resource);
            $scope.PlaceOptions = { watchEnter: true };
            this.SetupScope($scope, $rootScope, $location);
            if ($scope.CurentSearch == null)
                $scope.CurentSearch = new Application.SearchTravel();
            $scope.CurentSearch.Startpoint = new Application.MapPoint();
            $scope.CurentSearch.Endpoint = new Application.MapPoint();
            $scope.CurentSearch.StartAddress = $routeParams.from;
            if ($routeParams.flat && $routeParams.flat != "")
                $scope.CurentSearch.Startpoint.Latitude = parseFloat($routeParams.flat);
            if ($routeParams.flon && $routeParams.flon != "")
                $scope.CurentSearch.Startpoint.Longitude = parseFloat($routeParams.flon);
            if ($routeParams.fdate && $routeParams.fdate != "")
                if ($routeParams.fdate)
                    $scope.CurentSearch.Startdate = new Date($routeParams.fdate);
            $scope.CurentSearch.EndAddress = $routeParams.to;
            if ($routeParams.tlat && $routeParams.tlat != "")
                $scope.CurentSearch.Endpoint.Latitude = parseFloat($routeParams.tlat);
            if ($routeParams.tlon && $routeParams.tlon != "")
                $scope.CurentSearch.Endpoint.Longitude = parseFloat($routeParams.tlon);
            if ($routeParams.tdate && $routeParams.tdate != "")
                if ($routeParams.tdate)
                    $scope.CurentSearch.Enddate = new Date($routeParams.tdate);
            $scope.CurentSearch.Capacity = $routeParams.capacity ? parseInt($routeParams.capacity) : 1;
            $scope.CurentSearch.Direction = $routeParams.direction ? parseInt($routeParams.direction) : 1;
            $scope.CurentSearch.Luggage = $routeParams.luggage ? parseInt($routeParams.luggage) : 1;
            $scope.CurentSearch.Wait = $routeParams.wait ? parseInt($routeParams.wait) : 0;
            $scope.CurentSearch.Early = $routeParams.early ? parseInt($routeParams.early) : 0;
            $scope.Filter = {
                Saddress: $scope.CurentSearch.StartAddress,
                Slat: $scope.CurentSearch.Startpoint.Latitude,
                Slon: $scope.CurentSearch.Startpoint.Longitude,
                Sdate: ($scope.CurentSearch.Startdate) ? moment($scope.CurentSearch.Startdate).format("MM/DD/YYYY") : undefined,
                Eaddress: $scope.CurentSearch.EndAddress,
                Elat: $scope.CurentSearch.Endpoint.Latitude,
                Elon: $scope.CurentSearch.Endpoint.Longitude,
                Edate: ($scope.CurentSearch.Enddate) ? moment($scope.CurentSearch.Enddate).format("MM/DD/YYYY") : undefined,
                Direction: $scope.CurentSearch.Direction,
                Luggage: $scope.CurentSearch.Luggage,
                Wait: $scope.CurentSearch.Wait,
                Early: $scope.CurentSearch.Early,
                Capacity: $scope.CurentSearch.Capacity
            };
            if ($routeParams.search) {
                this.$travelService.GetAll($scope.Filter, function (result) {
                    $scope.Travels = result;
                    $scope.Travels.forEach(function (travel) {
                        _this.$userService.GetFriends(travel.DriverId, travel.Id, function (model) {
                            travel.Friends = model.Friends;
                        }, function (error) { console.log(error); });
                    });
                }, function (fail) { console.log(fail); });
            }
        }
        SearchController.prototype.SetupScope = function ($scope, $rootScope, $location) {
            $scope.ShowCalendar = function (pickername) {
                $rootScope.$broadcast("datetimepicker-show:" + pickername);
            };
            $scope.FindTravel = function () {
                $location.search('from', $scope.CurentSearch.StartAddress != "" ? $scope.CurentSearch.StartAddress : undefined);
                $location.search('flat', $scope.CurentSearch.Startpoint.Latitude ? $scope.CurentSearch.Startpoint.Latitude : undefined);
                $location.search('flon', $scope.CurentSearch.Startpoint.Longitude ? $scope.CurentSearch.Startpoint.Longitude : undefined);
                $location.search('fdate', $scope.CurentSearch.Startdate ? $scope.CurentSearch.Startdate : undefined);
                $location.search('to', $scope.CurentSearch.EndAddress != "" ? $scope.CurentSearch.EndAddress : undefined);
                $location.search('tlat', $scope.CurentSearch.Endpoint.Latitude ? $scope.CurentSearch.Endpoint.Latitude : undefined);
                $location.search('tlon', $scope.CurentSearch.Endpoint.Longitude ? $scope.CurentSearch.Endpoint.Longitude : undefined);
                $location.search('tdate', $scope.CurentSearch.Enddate ? $scope.CurentSearch.Enddate : undefined);
                $location.search('capacity', $scope.CurentSearch.Capacity);
                $location.search('direction', $scope.CurentSearch.Direction);
                $location.search('luggage', $scope.CurentSearch.Luggage);
                $location.search('wait', $scope.CurentSearch.Wait);
                $location.search('early', $scope.CurentSearch.Early);
                $location.search('search', "go");
            };
            $scope.ViewTravel = function (Id) { $location.path("/travel/view/" + Id.toString()); };
            $scope.GetDateString = function (date) { return moment(date).format('DD MM  YYYY'); };
            $scope.GetNameMappoint = function (name) {
                if (name)
                    return name.split(",");
            };
        };
        SearchController.$viewtemplate = "/Content/Templates/Search.html";
        return SearchController;
    })();
    Application.SearchController = SearchController;
    SearchController['$inject'] = ['$scope', '$rootScope', '$routeParams', '$resource', '$location'];
})(Application || (Application = {}));
//# sourceMappingURL=SearchController.js.map