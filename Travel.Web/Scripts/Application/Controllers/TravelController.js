/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var TravelController = (function () {
        function TravelController($scope, $rootScope, $resource, $location, $routeParams, $window) {
            var _this = this;
            $scope.CurentUserId = Application.AccountController.UserId;
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
                this.$travelService = new Application.TravelService($resource);
            if (this.$userService == null)
                this.$userService = new Application.UserService($resource);
            if (this.$commentService == null)
                this.$commentService = new Application.CommentService($resource);
            this.$travelService.Get(parseInt($routeParams.id), function (model) {
                $scope.CurrentTravel = model;
                _this.SetupGoogleMaps($scope);
                var index = $scope.CurrentTravel.Travelers
                    .map(function (x) { return x.Usertype; })
                    .indexOf(Application.UserType.Driver);
                if (index != -1) {
                    $scope.Driver = $scope.CurrentTravel.Travelers[index].User;
                    _this.$userService.GetFriends($scope.Driver.Id, $scope.CurrentTravel.Id, function (model) {
                        $scope.Friends = model;
                    }, function (error) { console.log(error); });
                }
                else
                    console.log('Driver not found');
            }, function (error) { console.log(error); });
            $scope.MapOptions = {
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            this.SetupScope($scope, $rootScope, $location, $window);
            this.SetupListener($scope, $rootScope);
        }
        TravelController.prototype.SetupScope = function ($scope, $rootScope, $location, $window) {
            var _this = this;
            $scope.TravelSubscription = function () {
                _this.$userService.NotifyPassenger($scope.Driver.Id, $scope.CurrentTravel.Id, function (passenger) { $scope.GetSubscribeForm(); }, function (error) { console.log(error); });
                _this.$userService.NotifyDriver($scope.Driver.Id, $scope.CurrentTravel.Id, function (driver) { }, function (error) { console.log(error); });
            };
            $scope.GetAllRating = function (type) {
                if ($scope.Driver) {
                    return $scope.Driver.Comments.filter(function (comment) { return comment.Type == type; }).length;
                }
            };
            $scope.GetLastSevenDaysRating = function (type) {
                if ($scope.Driver) {
                    var sevenday = moment().add('days', -7);
                    return $scope.Driver.Comments.filter(function (comment) {
                        return comment.Type == type
                            && (sevenday.isBefore(comment.Data)
                                || sevenday.isSame(comment.Data));
                    }).length;
                }
            };
            $scope.SendMessage = function () {
                $scope.CurrentComment.UserId = $scope.Driver.Id;
                if ($scope.CurrentComment.Message && $scope.CurrentComment.Message.match(/\w/)) {
                    _this.$commentService.Create($scope.CurrentComment, function (model) {
                        if (!$scope.Driver.Comments)
                            $scope.Driver.Comments = new Array();
                        $scope.Driver.Comments.unshift(model);
                        $scope.CloseMessageForm();
                    }, function (error) { console.log(error); });
                }
            };
            $scope.GetDateString = function (date) { return moment(date).format('DD MM  YYYY'); };
            $scope.GetTimeString = function (time) { return moment(time).format('hh:mm'); };
            $scope.GetCommentType = function (comment) {
                switch (comment) {
                    case Application.CommentType.Positive:
                        return "Позитивний";
                        break;
                    case Application.CommentType.Neutral:
                        return "Нейтральний";
                        break;
                    case Application.CommentType.Negative:
                        return "Негативний";
                        break;
                }
            };
            $scope.GetSubscribeForm = function () {
                if (Application.AccountController.UserId)
                    $scope.SubscribeForm.modal('show');
                else
                    $scope.AuthorizationForm.modal('show');
            };
            $scope.GetAllRatingForm = function () {
                $scope.RatingForm.on('hidden.bs.modal', function () {
                    setTimeout(function () { $scope.AllRatingForm.modal('show'); }, 1);
                    $scope.RatingForm.off();
                });
                $scope.RatingForm.modal('hide');
            };
            $scope.CloseAllRatingForm = function () {
                $scope.AllRatingForm.on('hidden.bs.modal', function () {
                    setTimeout(function () { $scope.RatingForm.modal('show'); }, 1);
                    $scope.AllRatingForm.off();
                });
                $scope.AllRatingForm.modal('hide');
            };
            $scope.GetMessageForm = function () {
                $scope.CurrentComment = new Application.Comment();
                $scope.CurrentComment.Type = Application.CommentType.Positive;
                if ($scope.AllRatingForm.is(":visible")) {
                    $scope.AllRatingForm.on('hidden.bs.modal', function () {
                        setTimeout(function () { $scope.MessageForm.modal('show'); }, 1);
                        $scope.AllRatingForm.off();
                    });
                    $scope.AllRatingForm.modal('hide');
                }
                if ($scope.RatingForm.is(":visible")) {
                    $scope.RatingForm.on('hidden.bs.modal', function () {
                        setTimeout(function () { $scope.MessageForm.modal('show'); }, 1);
                        $scope.RatingForm.off();
                    });
                    $scope.RatingForm.modal('hide');
                }
            };
            $scope.GetRatingForm = function () {
                _this.$commentService.GetAll({ userId: $scope.Driver.Id }, function (comments) { $scope.Driver.Comments = comments; }, function (reason) { console.log(reason); });
                $scope.RatingForm.modal('show');
            };
            $scope.CloseMessageForm = function () {
                $scope.MessageForm.on('hidden.bs.modal', function () {
                    setTimeout(function () { $scope.AllRatingForm.modal('show'); }, 1);
                    $scope.MessageForm.off();
                });
                $scope.MessageForm.modal('hide');
            };
            $scope.GoBack = function () {
                window.history.back();
            };
            $scope.GetNameMappoint = function (name) {
                if (name)
                    return name.split(",");
            };
        };
        TravelController.prototype.SetupListener = function ($scope, $rootScope) {
            $(window).resize(function () {
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
        TravelController.prototype.SetupGoogleMaps = function ($scope) {
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
        };
        TravelController.$viewtemplate = "/Content/Templates/Travel.html";
        TravelController.$helptemplate = "/Content/Templates/Travel.html";
        return TravelController;
    })();
    Application.TravelController = TravelController;
    TravelController['$inject'] = ['$scope', '$rootScope', '$resource', '$location', '$routeParams', '$window'];
})(Application || (Application = {}));
//# sourceMappingURL=TravelController.js.map