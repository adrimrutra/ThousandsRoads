/// <reference path="../_references.ts"/>
var Application;
(function (Application) {
    var NewTravelController = (function () {
        function NewTravelController($scope, $rootScope, $window, $resource, $routeParams) {
            $scope.Mapsnapshot = "http://maps.googleapis.com/maps/api/staticmap?";
            $scope.maptype = "maptype=roadmap";
            $scope.size = "&size=340x340";
            $scope.scale = "&scale=2";
            $scope.style = "&style=feature:road.highway%7Celement:geometry%7Cvisibility:simplified%7Ccolor:0xc280e9";
            $scope.style += "&style=feature:transit.line%7Cvisibility:simplified%7Ccolor:0xbababa";
            $scope.style += "&&style=feature:road.highway%7Celement:labels.text.stroke%7Cvisibility:on%7Ccolor:0xb06eba";
            $scope.style += "&style=feature:road.highway%7Celement:labels.text.fill%7Cvisibility:on%7Ccolor:0xffffff";
            $scope.Mapsnapshot += $scope.maptype;
            $scope.Mapsnapshot += $scope.size;
            $scope.Mapsnapshot += $scope.scale;
            $scope.Mapsnapshot += $scope.style;
            $scope.SnapMarker = "&markers=size:blue%7Clabel:S%7C";
            $scope.SnapPath = "&path=color:0x0000ff|weight:4";
            if (this.$travelService == null) {
                this.$travelService = new Application.TravelService($resource);
            }
            if (this.$travelerService == null) {
                this.$travelerService = new Application.TravelerService($resource);
            }
            if ($routeParams.id) {
                this.$travelService.Get(parseInt($routeParams.id), function (model) {
                    $scope.CurrentTravel = model;
                    //dodalu markeru do nashoi kartu
                    if ($scope.CurrentTravel.MapPoints) {
                        $scope.CurrentTravel.MapPoints.forEach(function (x) {
                            x.Marker = new google.maps.Marker({
                                map: $scope.RoadMap,
                                position: new google.maps.LatLng(x.Latitude, x.Longitude, false)
                            });
                            $scope.MapOptions.center = x.Marker.getPosition();
                            $scope.RoadMap.setCenter(x.Marker.getPosition());
                        });
                    }
                }, function (error) { console.log(error); });
            }
            else {
                $scope.CurrentTravel = new Application.Travel();
                if ($scope.CurrentTravel.Luggage == null) {
                    $scope.CurrentTravel.Luggage = Application.LuggageType.Min;
                }
            }
            this.SetupScope($scope, $rootScope, $window, $routeParams);
            this.SetupListener($scope, $rootScope);
            $scope.PlacesOptions = { watchEnter: true }; //types: ['geocode']
            $scope.Address = "";
            this.SetupGoogleMaps($scope, $routeParams.id === undefined);
        }
        NewTravelController.prototype.SetupListener = function ($scope, $rootScope) {
            $scope.$watch("PlacesDetails", function () {
                if ($scope.PlacesDetails)
                    $scope.RoadMap.setCenter($scope.PlacesDetails.geometry.location);
            });
            // console.log();
        };
        NewTravelController.prototype.SetupGoogleMaps = function ($scope, needAutoLocation) {
            $scope.Geocoder = new google.maps.Geocoder();
            $scope.MapOptions = {
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var initialLocation;
            var browserSupportFlag = new Boolean();
            // Try W3C Geolocation (Preferred)
            if (needAutoLocation) {
                if (navigator.geolocation) {
                    browserSupportFlag = true;
                    navigator.geolocation.getCurrentPosition(function (position) {
                        initialLocation = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
                        $scope.MapOptions.center = initialLocation;
                        $scope.RoadMap.setCenter(initialLocation);
                    }, function () {
                        // handleNoGeolocation(browserSupportFlag);
                    });
                }
                else {
                    browserSupportFlag = false;
                    navigator.geolocation.getCurrentPosition(function (position) {
                        $(document).ready(function () {
                            $.getJSON("http://jsonip.appspot.com/?callback=?", function (data) {
                                jQuery.get("http://ipinfo.io/" + data.ip, function (response) {
                                    var lats = response.loc.split(',')[0];
                                    var lngs = response.loc.split(',')[1];
                                    var location = new google.maps.LatLng(lats, lngs);
                                    $scope.MapOptions.center = location;
                                    $scope.RoadMap.setCenter(location);
                                    $scope.RoadMap.setZoom(5);
                                }, "jsonp");
                            });
                        });
                    });
                }
            }
        };
        NewTravelController.prototype.SetupScope = function ($scope, $rootScope, $window, $routeParams) {
            var _this = this;
            $scope.GetNameMappoint = function (name) {
                if (name)
                    return name.split(",");
            };
            //---------------------------------------------------------
            $scope.ShowCalendar = function (pickername) {
                $rootScope.$broadcast("datetimepicker-show:" + pickername);
            };
            //---------------------------------------------------------
            $scope.AddMarker = function ($event, $params) {
                if (!$scope.CurrentTravel.MapPoints)
                    $scope.CurrentTravel.MapPoints = new Array();
                var point = new Application.MapPoint();
                var marker = new google.maps.Marker({
                    map: $scope.RoadMap,
                    position: $params[0].latLng
                });
                point.Latitude = marker.getPosition().lat();
                point.Longitude = marker.getPosition().lng();
                point.Name = "Loading...";
                point.Marker = marker;
                $scope.CurrentTravel.MapPoints.push(point);
                $scope.GetPointInfo($params[0].latLng, "", point);
            };
            $scope.DeleteMapPoint = function (index) {
                var point = $scope.CurrentTravel.MapPoints[index];
                point.Marker.setMap(null);
                delete point.Marker;
                $scope.CurrentTravel.MapPoints.splice(index, 1);
            };
            $scope.GetPointInfo = function (location, address, point) {
                $scope.Geocoder.geocode({ 'location': location, 'address': address }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (point)
                            point.Name = results[0].formatted_address;
                        $scope.RoadMap.setCenter(location);
                    }
                    else {
                        if (point) {
                            point.Name = "Address not found";
                        }
                    }
                });
            };
            $scope.CancelTravel = function () { $window.history.back(); };
            $scope.SaveTravel = function () {
                if ($routeParams.id) {
                    $scope.CurrentTravel.MapPoints.forEach(function (point) {
                        delete point.Marker;
                    });
                    //=============================================================================
                    $scope.CurrentTravel.MapPoints.forEach(function (point) {
                        if ($scope.SnapMarker[$scope.SnapMarker.length - 1] != "|" && $scope.SnapMarker[$scope.SnapMarker.length - 1] != "C")
                            $scope.SnapMarker += "|";
                        if ($scope.SnapMarker[$scope.SnapPath.length - 1] != "|" && $scope.SnapMarker[$scope.SnapPath.length - 1] != "4")
                            $scope.SnapPath += "|";
                        $scope.SnapMarker += point.Latitude.toFixed(6).toString() + ',' + point.Longitude.toFixed(6).toString();
                        $scope.SnapPath += point.Latitude.toFixed(6).toString() + ',' + point.Longitude.toFixed(6).toString();
                    });
                    $scope.Mapsnapshot += $scope.SnapMarker;
                    $scope.Mapsnapshot += $scope.SnapPath;
                    $scope.Mapsnapshot += "&maptype=roadmap&format=png32&sensor=true";
                    $scope.CurrentTravel.Mapsnapshot = null;
                    $scope.CurrentTravel.Mapsnapshot = $scope.Mapsnapshot;
                    //=============================================================================
                    $scope.CurrentTravel.Startdate.setHours($scope.TmpStartDate.getHours());
                    $scope.CurrentTravel.Startdate.setMinutes($scope.TmpStartDate.getMinutes());
                    //  $scope.CurrentTravel.Startdate.se
                    //=============================================================================
                    _this.$travelService.Update($scope.CurrentTravel, function (model) { $window.history.back(); }, function (error) { console.log(error); });
                }
                else {
                    if (Application.AccountController.UserId) {
                        $scope.CurrentTravel.Travelers = new Array();
                        var currentTraveler = new Application.Traveler();
                        currentTraveler.UserId = Application.AccountController.UserId;
                        currentTraveler.TravelId = $scope.CurrentTravel.Id;
                        currentTraveler.Usertype = Application.UserType.Driver;
                        $scope.CurrentTravel.Travelers.push(currentTraveler);
                        //------------------------------------------------------------
                        $scope.CurrentTravel.MapPoints.forEach(function (point) {
                            delete point.Marker;
                        });
                        //=============================================================================
                        $scope.CurrentTravel.MapPoints.forEach(function (point) {
                            if ($scope.SnapMarker[$scope.SnapMarker.length - 1] != "|" && $scope.SnapMarker[$scope.SnapMarker.length - 1] != "C")
                                $scope.SnapMarker += "|";
                            if ($scope.SnapMarker[$scope.SnapPath.length - 1] != "|" && $scope.SnapMarker[$scope.SnapPath.length - 1] != "4")
                                $scope.SnapPath += "|";
                            $scope.SnapMarker += point.Latitude.toFixed(6).toString() + ',' + point.Longitude.toFixed(6).toString();
                            $scope.SnapPath += point.Latitude.toFixed(6).toString() + ',' + point.Longitude.toFixed(6).toString();
                        });
                        $scope.Mapsnapshot += $scope.SnapMarker;
                        $scope.Mapsnapshot += $scope.SnapPath;
                        $scope.Mapsnapshot += "&maptype=roadmap&format=png32&sensor=true";
                        //=============================================================================
                        $scope.CurrentTravel.Mapsnapshot = $scope.Mapsnapshot;
                        _this.$travelService.Create($scope.CurrentTravel, function (model) { $window.history.back(); }, function (error) { console.log(error); });
                    }
                }
            };
        };
        NewTravelController.$viewtemplate = "/Content/Templates/NewTravel.html";
        return NewTravelController;
    })();
    Application.NewTravelController = NewTravelController;
    NewTravelController['$inject'] = ['$scope', '$rootScope', '$window', '$resource', '$routeParams'];
})(Application || (Application = {}));
//# sourceMappingURL=NewTravelController.js.map