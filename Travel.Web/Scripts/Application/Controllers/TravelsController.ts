/// <reference path="../_references.ts" />
module Application {
    export interface ITravelsScope extends ng.IScope {
        Travels: Array<Travel>;
        Dravers: Array<User>;
        AddTravel: Function;
        EditTravel: Function;
        DeleteTravel: Function;
        ViewTravel: Function;
        GetDateString: Function;
        GetNameMappoint: Function;
        CurentUserId: number;
        //AllTravelsFriends: Array<TravelFriends>;
        DisplayName: Function;
    }

    export class TravelsController {
        public static $viewtemplate = "/Content/Templates/Travels.html";
        public static $helptemplate = "/Content/Templates/TravelsHelp.html";
        private $travelService: TravelService;
        private $userService: UserService;

        constructor($scope: ITravelsScope, $rootScope: ng.IScope, $resource: IResourceService, $location: ng.ILocationService) {
            $scope.CurentUserId = AccountController.UserId; 
            if (this.$travelService == null)
                this.$travelService = new TravelService($resource);
            if (this.$userService == null)
                this.$userService = new UserService($resource);
            //if ($scope.AllTravelsFriends)
            //    {
            //    console.log("VUDILUV======>");
            //        $scope.AllTravelsFriends = new Array<TravelFriends>();
            //}


            this.$travelService.GetAll({}, (modules: Travel[]) => {
                $scope.Travels = modules;
                $scope.Travels.forEach((travel) => {
                    this.$userService.GetFriends(travel.DriverId, travel.Id,
                        (model: TravelFriends) => {                 
                            travel.Friends = model.Friends;
                        },
                        (error) => { console.log(error) }
                        );
                });
            }, (error) => console.log(error));


            $scope.DisplayName = (value) => {
                if (value)
                {
                    var values = value.split(' ');
                    return values[0][0] + ". " +values[1];
                }
            };
                

            this.SetupScope($scope, $rootScope, $location);
            
  
        }
        SetupScope($scope: ITravelsScope, $rootScope: ng.IScope, $location: ng.ILocationService) {

            $scope.AddTravel = () => { $location.path("/travel/new"); }
            $scope.EditTravel = (Id: number) => { $location.path("/travel/edit/" + Id.toString()); }

            $scope.DeleteTravel = (model: Travel) => {
                this.$travelService.Delete(model,
                    () => {
                        var index = $scope.Travels.map((value) => { return value.Id }).indexOf(model.Id);
                        $scope.Travels.splice(index, 1);
                    },
                    (error) => { console.log(error) }
                    );
            };


            $scope.GetNameMappoint = (name: string) => {
                if (name)
                    return name.split(",");
            };


            $scope.ViewTravel = (Id: number) => { $location.path("/travel/view/" + Id.toString()); }

            $scope.GetDateString = (date: Date) => {return moment(date).format('DD MM  YYYY') };

         
            $rootScope.$on("event:logining", (data, ...args) => {
                $scope.CurentUserId = AccountController.UserId;   

            });
            $rootScope.$on("event:logout", (data, ...args) => {
                $scope.CurentUserId = undefined;
            });

        }
    }
    TravelsController['$inject'] = ['$scope', '$rootScope', '$resource', '$location'];
}