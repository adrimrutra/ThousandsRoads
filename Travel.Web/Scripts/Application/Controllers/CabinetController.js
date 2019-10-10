/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var CabinetController = (function () {
        function CabinetController($scope, $rootScope, $resource, $location, $routeParams) {
            if (this.$userService == null)
                this.$userService = new Application.UserService($resource);
            if (this.$travelService == null)
                this.$travelService = new Application.TravelService($resource);
            if (this.$commentService == null)
                this.$commentService = new Application.CommentService($resource);
            this.SetupScope($scope, $rootScope, $resource, $location, $routeParams);
            this.SetupListener($scope, $rootScope, $resource, $location, $routeParams);
            $scope.GetMailForm();
        }
        CabinetController.prototype.SetupScope = function ($scope, $rootScope, $resource, $location, $routeParams) {
            //$scope.ApplyUser = (model: User) => {
            //    $scope.CurentUser = model;
            //    $scope.CurentUser.Avatar = $scope.CurentUser.Avatar;
            //};
            var _this = this;
            this.$userService.GetCurenUser(function (model) {
                $scope.CurentUser = model;
                $scope.Avatar = model.Avatar;
            }, function (error) { console.log(error); });
            this.$userService.GetMessages(function (messages) {
                $scope.CurentUserMessages = messages;
                $scope.CountNewMessages = function () {
                    var count = 0;
                    $scope.CurentUserMessages.forEach(function (message) {
                        if (!message.State) {
                            count++;
                        }
                    });
                    return count;
                };
            }, function (error) { console.log(error); });
            $scope.InviteFriend = function (id) {
                if (_this.$socialAccount == null)
                    _this.$socialAccount = Application.SocialAccount.GetAccount();
                _this.$socialAccount.InviteFriend(id).done(function (avatar) {
                });
            };
            $scope.GetMailForm = function () {
                $scope.IsMessagePage = true;
                $scope.GetPageTemplate = function () {
                    var v = $routeParams;
                    return '/Content/Templates/CabinetMails.html';
                };
            };
            $scope.GetMyinfoForm = function () {
                $scope.IsMessagePage = false;
                $scope.GetPageTemplate = function () {
                    var v = $routeParams;
                    return '/Content/Templates/CabinetMyinfo.html';
                };
            };
            $scope.GetFriendForm = function () {
                $scope.IsMessagePage = false;
                if (!$scope.CurentUserFriends) {
                    if (_this.$socialAccount == null)
                        _this.$socialAccount = Application.SocialAccount.GetAccount();
                    _this.$socialAccount.GetFriends().done(function (friends) {
                        $scope.$apply(function () {
                            $scope.CurentUserFriends = friends;
                        });
                    });
                }
                $scope.GetPageTemplate = function () {
                    var v = $routeParams;
                    return '/Content/Templates/CabinetFriends.html';
                };
            };
            $scope.GetCommentForm = function () {
                $scope.IsMessagePage = false;
                if ($scope.CurentUser.Comments) {
                    _this.$commentService.GetAll({ userId: $scope.CurentUser.Id }, function (comments) { $scope.CurentUser.Comments = comments; }, function (reason) { console.log(reason); });
                }
                $scope.GetPageTemplate = function () {
                    var v = $routeParams;
                    return '/Content/Templates/CabinetComments.html';
                };
            };
            $scope.DeleteMessages = function () {
                _this.$userService.DeleteMessages($scope.DeletedIds, function () {
                    $scope.DeletedIds.forEach(function (id) {
                        var index = $scope.CurentUserMessages.map(function (message) { return message.Id; }).indexOf(id);
                        $scope.CurentUserMessages.splice(index, 1);
                    });
                }, function (reason) { console.log(reason); });
            };
            $scope.AddPopId = function (flag, Id) {
                if (!$scope.DeletedIds)
                    $scope.DeletedIds = new Array();
                if (flag == true) {
                    $scope.DeletedIds.push(Id);
                }
                if (flag == false) {
                    angular.forEach($scope.DeletedIds, function (deleteId) {
                        if (deleteId == Id) {
                            $scope.DeletedIds.splice($scope.DeletedIds.indexOf(deleteId), 1);
                        }
                    });
                }
            };
            $scope.MessageRead = function (id) {
                var message = $scope.CurentUserMessages.filter(function (message) {
                    return message.Id == id;
                })[0];
                message.State = true;
                _this.$userService.PutMessage(message, function (message) { }, function (reason) { console.log(reason); });
            };
            $scope.GetCustomPegeFriend = function (query) {
                if (_this.$socialAccount == null)
                    _this.$socialAccount = Application.SocialAccount.GetAccount();
                _this.$socialAccount.GetCustom(query).done(function (friends) {
                    $scope.$apply(function () { $scope.CurentUserFriends = friends; });
                });
            };
            $scope.SendMessage = function (flag, id) {
                var message = $scope.CurentUserMessages.filter(function (message) {
                    return message.Id == id;
                })[0];
                var tmp = new Application.Message();
                tmp.Type = Application.MessageType.Inner;
                tmp.UserId = message.MessengerId;
                tmp.UserEmail = message.MessengerEmail;
                tmp.MessengerId = message.UserId;
                tmp.MessengerEmail = message.UserEmail;
                if (flag == true) {
                    message.Type = Application.MessageType.Accept;
                    _this.$userService.PutMessage(message, function (message) { }, function (reason) { console.log(reason); });
                    _this.$travelService.PutTraveler(message.TravelId, message.UserId, function () { }, function (error) { console.log(error); });
                    tmp.Theme = "Підтвердження на поїздку";
                    tmp.MessageText = "Я беру вас в поїздку із задоволенням !";
                    _this.$userService.PostMessage(tmp, function (message) { }, function (reason) { console.log(reason); });
                }
                if (flag == false) {
                    message.Type = Application.MessageType.Decline;
                    _this.$userService.PutMessage(message, function (message) { }, function (reason) { console.log(reason); });
                    tmp.Theme = "Відмова на поїздку";
                    tmp.MessageText = "Вибачте, Ваше прохання про поїздку відхилине.";
                    _this.$userService.PostMessage(tmp, function (message) { }, function (reason) { console.log(reason); });
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
        };
        CabinetController.prototype.SetupListener = function ($scope, $rootScope, $resource, $location, $routeParams) {
        };
        CabinetController.$viewtemplate = "/Content/Templates/Cabinet.html";
        return CabinetController;
    })();
    Application.CabinetController = CabinetController;
    CabinetController['$inject'] = ['$scope', '$rootScope', '$resource', '$location', '$routeParams'];
})(Application || (Application = {}));
//# sourceMappingURL=CabinetController.js.map