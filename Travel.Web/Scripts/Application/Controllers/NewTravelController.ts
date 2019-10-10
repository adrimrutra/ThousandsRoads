/// <reference path="../_references.ts"/>

module Application {
    export interface INewTravelScope extends ng.IScope {
        SaveTravel: Function;
        CancelTravel: Function;
        CurrentTravel: Travel;
        AddMarker: Function;
        MapOptions: google.maps.MapOptions;
        RoadMap: google.maps.Map;
        Geocoder: google.maps.Geocoder;
        Address: string;
        GetPointInfo: Function;
        FunctionWithoutTS: Function;
        PlacesOptions: any;
        PlacesDetails: google.maps.places.PlaceResult;
        Mapsnapshot: string;
        SnapMarker: string;
        SnapPath: string;
        maptype: string;
        size: string;
        scale: string;
        style: string;
        ShowCalendar: Function;
        GetNameMappoint: Function;
        CurentLuggage: LuggageType;
        DeleteMapPoint: Function;
        TmpStartDate: Date;

    }

    export class NewTravelController {
        public static $viewtemplate = "/Content/Templates/NewTravel.html";
        private $travelService: TravelService;
        private $travelerService: TravelerService;

        constructor($scope: INewTravelScope, $rootScope: ng.IScope, $window: ng.IWindowService, $resource: IResourceService, $routeParams: IRouteParams) {

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
                this.$travelService = new TravelService($resource);
            }
            if (this.$travelerService == null) {
                this.$travelerService = new TravelerService($resource);
            }

            
            if ($routeParams.id) {
                this.$travelService.Get(parseInt($routeParams.id),
                    (model: Travel) => {
                        $scope.CurrentTravel = model;
                     
                        //dodalu markeru do nashoi kartu
                        if ($scope.CurrentTravel.MapPoints) {
                            $scope.CurrentTravel.MapPoints.forEach((x) => {
                                x.Marker = new google.maps.Marker({
                                    map: $scope.RoadMap,
                                    position: new google.maps.LatLng(x.Latitude, x.Longitude, false)
                                });
                                $scope.MapOptions.center = x.Marker.getPosition();
                                $scope.RoadMap.setCenter(x.Marker.getPosition());
                            });
                        }
                    },
                    (error) => { console.log(error) });
            }
            else {

                $scope.CurrentTravel = new Travel();      
                if ($scope.CurrentTravel.Luggage == null) {
                    $scope.CurrentTravel.Luggage = LuggageType.Min;
                }
            }

            this.SetupScope($scope, $rootScope, $window, $routeParams);
            this.SetupListener($scope, $rootScope);
            $scope.PlacesOptions = { watchEnter: true };//types: ['geocode']
            $scope.Address = "";
            this.SetupGoogleMaps($scope, $routeParams.id === undefined);
        }

        SetupListener($scope: INewTravelScope, $rootScope) {
            $scope.$watch("PlacesDetails", () => {
                if ($scope.PlacesDetails)
                    $scope.RoadMap.setCenter($scope.PlacesDetails.geometry.location);
            });
          
           // console.log();





        }

        SetupGoogleMaps($scope: INewTravelScope, needAutoLocation: boolean) {
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
                // Browser doesn't support Geolocation
                else {

                    browserSupportFlag = false;

                    navigator.geolocation.getCurrentPosition((position) => {
                        $(document).ready(() => {
                            $.getJSON("http://jsonip.appspot.com/?callback=?",
                                (data) => {
                                    jQuery.get("http://ipinfo.io/" + data.ip, (response) => {
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
        }

        SetupScope($scope: INewTravelScope, $rootScope: ng.IScope, $window: ng.IWindowService, $routeParams: IRouteParams) {

            $scope.GetNameMappoint = (name: string) => {
                if (name)
                    return name.split(",");
            };
            //---------------------------------------------------------

            $scope.ShowCalendar = (pickername: string) => {
                $rootScope.$broadcast("datetimepicker-show:" + pickername);
            }
                //---------------------------------------------------------

            $scope.AddMarker = function ($event, $params) {
                if (!$scope.CurrentTravel.MapPoints)
                    $scope.CurrentTravel.MapPoints = new Array<MapPoint>();
                var point = new MapPoint();
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

            $scope.DeleteMapPoint = (index: number) => {
                var point = $scope.CurrentTravel.MapPoints[index];
                point.Marker.setMap(null);
                delete point.Marker;
             
                $scope.CurrentTravel.MapPoints.splice(index, 1);
            };

            $scope.GetPointInfo = (location: google.maps.LatLng, address: string, point: MapPoint) => {
                $scope.Geocoder.geocode({ 'location': location, 'address': address }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        
                        if (point)
                            point.Name = results[0].formatted_address;

                        $scope.RoadMap.setCenter(location);
                       

                        
                    } else {
                        if (point) {
                            point.Name = "Address not found";
                        }
                    }

                })
            }


            $scope.CancelTravel = () => { $window.history.back(); }


            $scope.SaveTravel = () => {
                if ($routeParams.id) {
                    
                    $scope.CurrentTravel.MapPoints.forEach((point) => {
                        delete point.Marker;
                    });

                    //=============================================================================
                    $scope.CurrentTravel.MapPoints.forEach((point) => {
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

                    this.$travelService.Update($scope.CurrentTravel,
                        (model: Travel) => { $window.history.back(); },
                        (error) => { console.log(error) });
                }
                else {
                    if (AccountController.UserId) {
                        $scope.CurrentTravel.Travelers = new Array<Traveler>();
                        var currentTraveler = new Traveler();
                        currentTraveler.UserId = AccountController.UserId;
                        currentTraveler.TravelId = $scope.CurrentTravel.Id;
                        currentTraveler.Usertype = UserType.Driver;
                        $scope.CurrentTravel.Travelers.push(currentTraveler);
                        //------------------------------------------------------------
                        $scope.CurrentTravel.MapPoints.forEach((point) => {
                            delete point.Marker;
                        });

                        //=============================================================================
                        $scope.CurrentTravel.MapPoints.forEach((point) => {
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
                         
                        this.$travelService.Create($scope.CurrentTravel,
                            (model: Travel) => { $window.history.back(); },
                            (error) => { console.log(error) });
                    }
                }
            }
        }
    }
    NewTravelController['$inject'] = ['$scope', '$rootScope', '$window', '$resource', '$routeParams'];
}
