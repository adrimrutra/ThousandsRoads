/// <reference path="../_references.ts" />
module Application {
    export interface ITravelScope extends ng.IScope {
        CurrentTravel: Travel;
        CurrentComment: Comment;
        CommentType: CommentType;
        GetCommentType: Function;
        Driver: User;
        SendMessage: Function;
        GetDateString: Function;
        GetTimeString: Function;
        RoadMap: google.maps.Map;
        Geocoder: google.maps.Geocoder;
        MapOptions: google.maps.MapOptions;
        SubscribeForm: JQuery;
        AuthorizationForm: JQuery;
        RatingForm: JQuery;
        AllRatingForm: JQuery;
        GetSubscribeForm: Function;
        GetRatingForm: Function;
        GetAllRatingForm: Function;
        CloseAllRatingForm: Function;
        GoBack: Function;
        GetNameMappoint: Function;
        Message: string;
        MessageForm: JQuery;
        GetMessageForm: Function;
        CloseMessageForm: Function;
        GetTypeResponse: Function;
        AddComment: Function;
        Button: Object;
        GetAllRating: Function;
        GetLastSevenDaysRating: Function;
        TravelSubscription: Function;
        Friends: TravelFriends;
        CurentUserId: number;

    }

    export class TravelController {
        public static $viewtemplate = "/Content/Templates/Travel.html";
        public static $helptemplate = "/Content/Templates/Travel.html";
        private $travelService: TravelService;
        private $userService: UserService;
        private $commentService: CommentService;
        constructor($scope: ITravelScope, $rootScope: ng.IScope, $resource: IResourceService, $location: ng.ILocationService, $routeParams: IRouteParams, $window: ng.IWindowService) {
            $scope.CurentUserId = AccountController.UserId; 

            var height = $('#cont').height();
            // console.log('---->', height);

            var heightpoint = $('#ul-point').height();
            // console.log('---->', heightpoint);

            var heighttruepoint = $('#point-container').height();
            //console.log('---->', heighttruepoint);

            $("#selectable").css({ height: (height - (heighttruepoint + 85)) });




            $scope.SubscribeForm = $('#subscribe').modal('hide');
            $scope.AuthorizationForm = $('#authorization').modal('hide');
            $scope.RatingForm = $('#rating').modal('hide');
            $scope.AllRatingForm = $('#all-rating').modal('hide');
            $scope.MessageForm = $('#message-form').modal('hide');

            if (this.$travelService == null)
                this.$travelService = new TravelService($resource);
            if (this.$userService == null)
                this.$userService = new UserService($resource);
            if (this.$commentService == null)
                this.$commentService = new CommentService($resource);

            this.$travelService.Get(parseInt($routeParams.id),
                (model: Travel) => {
                    $scope.CurrentTravel = model;
                    this.SetupGoogleMaps($scope);

                    var index = $scope.CurrentTravel.Travelers
                        .map((x) => { return x.Usertype; })
                        .indexOf(UserType.Driver);
                    if (index != -1) {
                        $scope.Driver = $scope.CurrentTravel.Travelers[index].User;
                        

                        this.$userService.GetFriends($scope.Driver.Id, $scope.CurrentTravel.Id,
                            (model: TravelFriends) => {
                                $scope.Friends = model;
                            },
                            (error) => { console.log(error) }
                            );
                    }
                    else
                        console.log('Driver not found');
                },
                (error) => { console.log(error) });

            $scope.MapOptions = {
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            this.SetupScope($scope, $rootScope, $location, $window);
            this.SetupListener($scope, $rootScope);
        }



        SetupScope($scope: ITravelScope, $rootScope: ng.IScope, $location: ng.ILocationService, $window: ng.IWindowService) {

            $scope.TravelSubscription = () => {


                this.$userService.NotifyPassenger(
                    $scope.Driver.Id, $scope.CurrentTravel.Id,
                    (passenger: boolean) => { $scope.GetSubscribeForm(); },
                    (error) => { console.log(error) });

                this.$userService.NotifyDriver(
                    $scope.Driver.Id, $scope.CurrentTravel.Id,
                    (driver: boolean) => { },
                    (error) => { console.log(error) });


            };


            $scope.GetAllRating = (type: CommentType) => {
                if ($scope.Driver) {
                    return $scope.Driver.Comments.filter((comment: Comment) => { return comment.Type == type }).length;
                }
            };

            $scope.GetLastSevenDaysRating = (type: CommentType) => {
                if ($scope.Driver) {
                    var sevenday = moment().add('days', -7);
                    return $scope.Driver.Comments.filter((comment: Comment) => { return comment.Type == type
                        && (sevenday.isBefore(comment.Data)
                        || sevenday.isSame(comment.Data))
                    }).length;
                }
            };

            $scope.SendMessage = () => {
                $scope.CurrentComment.UserId = $scope.Driver.Id;
                if ($scope.CurrentComment.Message && $scope.CurrentComment.Message.match(/\w/)) {
                    this.$commentService.Create($scope.CurrentComment,
                        (model: Comment) => {
                                if (!$scope.Driver.Comments)
                                $scope.Driver.Comments = new Array<Comment>();
                            $scope.Driver.Comments.unshift(model);
                            $scope.CloseMessageForm();

                        },
                        (error) => { console.log(error) });
                }
            }
            $scope.GetDateString = (date: Date) => {return moment(date).format('DD MM  YYYY') };
            $scope.GetTimeString = (time: Date) => {return moment(time).format('hh:mm') };

            $scope.GetCommentType = (comment: CommentType) => {
                switch (comment) {
                    case CommentType.Positive: return "Позитивний"; break;
                    case CommentType.Neutral: return "Нейтральний"; break;
                    case CommentType.Negative: return "Негативний"; break;
                }
            };

            $scope.GetSubscribeForm = () => {
                if (AccountController.UserId)
                    $scope.SubscribeForm.modal('show');
                else
                    $scope.AuthorizationForm.modal('show');
            };
       


            $scope.GetAllRatingForm = () => {
                $scope.RatingForm.on('hidden.bs.modal', () => {
                    setTimeout(() => { $scope.AllRatingForm.modal('show'); }, 1);
                    $scope.RatingForm.off();
                });
                $scope.RatingForm.modal('hide');
            };


            $scope.CloseAllRatingForm = () => {
                $scope.AllRatingForm.on('hidden.bs.modal', () => {
                    setTimeout(() => { $scope.RatingForm.modal('show'); }, 1);
                    $scope.AllRatingForm.off();
                });
                $scope.AllRatingForm.modal('hide');
            };


            $scope.GetMessageForm = () => {
                
                $scope.CurrentComment = new Comment();
                $scope.CurrentComment.Type = CommentType.Positive;

                if ($scope.AllRatingForm.is(":visible")) {
                   
                    $scope.AllRatingForm.on('hidden.bs.modal', () => {
                        setTimeout(() => { $scope.MessageForm.modal('show'); }, 1);
                        $scope.AllRatingForm.off();

                    });
                    $scope.AllRatingForm.modal('hide');
                }
                if ($scope.RatingForm.is(":visible")) {
                    $scope.RatingForm.on('hidden.bs.modal', () => {
                        setTimeout(() => { $scope.MessageForm.modal('show'); }, 1);
                        $scope.RatingForm.off();
                    });
                    $scope.RatingForm.modal('hide');               
                }
            };

            $scope.GetRatingForm = () => {

                this.$commentService.GetAll({ userId: $scope.Driver.Id },
                    (comments: Array<Comment>) => { $scope.Driver.Comments = comments; },
                    (reason) => { console.log(reason); });
                $scope.RatingForm.modal('show');

            };




            $scope.CloseMessageForm = () => {

                $scope.MessageForm.on('hidden.bs.modal', () => {
                    setTimeout(() => { $scope.AllRatingForm.modal('show'); }, 1);
                    $scope.MessageForm.off();
                });
                $scope.MessageForm.modal('hide');
            };




            $scope.GoBack = () => {
                window.history.back();
            };

            $scope.GetNameMappoint = (name: string) => {
                if (name)
                    return name.split(",");
            };

        }
        SetupListener($scope, $rootScope) {



            $(window).resize(() => {

                var height = $('#cont').height();
                //console.log('---->', height);

                var heightpoint = $('#ul-point').height();
                //console.log('---->', heightpoint);

                var heighttruepoint = $('#point-container').height();
                //console.log('---->', heighttruepoint);

                $("#selectable").css({ height: (height - (heighttruepoint + 85)) });
                //$("#selectable").css({ height: 100 });

                //console.log('---->', $('#selectable').height());

                //$("#selectable").css({ height: height });

            });

            $rootScope.$on("event:logining", (data, ...args) => {
                $scope.CurentUserId = AccountController.UserId;

            });
            $rootScope.$on("event:logout", (data, ...args) => {
                $scope.CurentUserId = undefined;
            });



        }

        SetupGoogleMaps($scope: ITravelScope) {


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
        }

    }
    TravelController['$inject'] = ['$scope', '$rootScope', '$resource', '$location', '$routeParams', '$window'];
} 