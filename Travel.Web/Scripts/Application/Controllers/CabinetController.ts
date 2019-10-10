/// <reference path="../_references.ts" />
module Application {

    export interface ICabinetScope extends ng.IScope {

        CurentUserId: number;
        CurentUser: User;
        CurentUserMessages: Array<Message>;
        CurentUserFriends: SocialFriends;
        GetDateString: Function;
        GetTimeString: Function;
        GetPageTemplate: Function;
        GetMailForm: Function;
        GetFriendForm: Function;
        GetCommentForm: Function;
        GetMyinfoForm: Function;
        GetCommentType: Function;
        DeleteMessages: Function;
        DeletedIds: Array<number>;
        AddPopId: Function;
        MessageRead: Function;
        SendMessage: Function;
        InviteFriend: Function;
        CountNewMessages: Function;
        GetCustomPegeFriend: Function;
        IsMessagePage: boolean;
        ApplyUser: Function;
        Avatar: string;
    }
    export class CabinetController {
        public static $viewtemplate = "/Content/Templates/Cabinet.html";
        private $userService: UserService;
        private $socialAccount: ISocialScope;
        private $travelService: TravelService;
        private $commentService: CommentService;

        constructor($scope: ICabinetScope, $rootScope: ng.IScope, $resource: IResourceService, $location: ng.ILocationService, $routeParams: IRouteParams) {
            if (this.$userService == null)
                this.$userService = new UserService($resource);
            if (this.$travelService == null)
                this.$travelService = new TravelService($resource);
            if (this.$commentService == null)
                this.$commentService = new CommentService($resource);

            
            
            this.SetupScope($scope, $rootScope, $resource, $location, $routeParams);
            this.SetupListener($scope, $rootScope, $resource, $location, $routeParams);
            $scope.GetMailForm();
        }
        SetupScope($scope: ICabinetScope, $rootScope: ng.IScope, $resource: IResourceService, $location: ng.ILocationService, $routeParams: IRouteParams) {

            //$scope.ApplyUser = (model: User) => {
            //    $scope.CurentUser = model;
            //    $scope.CurentUser.Avatar = $scope.CurentUser.Avatar;
            //};

            this.$userService.GetCurenUser(
                (model: User) => {
                     $scope.CurentUser = model;
                     $scope.Avatar = model.Avatar;
                },

                (error) => { console.log(error) });

            this.$userService.GetMessages(
                (messages: Array<Message>) => {
                    $scope.CurentUserMessages = messages;

                    $scope.CountNewMessages = () => {
                        var count = 0;
                        $scope.CurentUserMessages.forEach((message) => {
                            if (!message.State) {
                                count++;
                            }
                        });
                        return count;
                    };
                },
                (error) => { console.log(error) }
                );

            $scope.InviteFriend = (id: number) => {
                if (this.$socialAccount == null)
                    this.$socialAccount = SocialAccount.GetAccount();
                this.$socialAccount.InviteFriend(id).done((avatar) => {

                });
            };

            $scope.GetMailForm = () => {
                $scope.IsMessagePage = true;
                $scope.GetPageTemplate = () => {
                    var v = $routeParams;
                    return '/Content/Templates/CabinetMails.html';
                }
            }
            $scope.GetMyinfoForm = () => {
                $scope.IsMessagePage = false;
                $scope.GetPageTemplate = () => {
                    var v = $routeParams;
                    return '/Content/Templates/CabinetMyinfo.html';
                }
            }
            $scope.GetFriendForm = () => {
                $scope.IsMessagePage = false;

                if (!$scope.CurentUserFriends) {
                    if (this.$socialAccount == null)
                        this.$socialAccount = SocialAccount.GetAccount();
                    this.$socialAccount.GetFriends().done((friends: SocialFriends) => {
                        $scope.$apply(() => {
                            $scope.CurentUserFriends = friends;
                        });
                    });
                }

                $scope.GetPageTemplate = () => {
                

                    var v = $routeParams;
                    return '/Content/Templates/CabinetFriends.html';
                }
            }

            $scope.GetCommentForm = () => {
                $scope.IsMessagePage = false;
                if ($scope.CurentUser.Comments) {
                    this.$commentService.GetAll({ userId: $scope.CurentUser.Id },
                        (comments: Array<Comment>) => { $scope.CurentUser.Comments = comments; },
                        (reason) => { console.log(reason); });
                }
                $scope.GetPageTemplate = () => {
                   
                    var v = $routeParams;
                    return '/Content/Templates/CabinetComments.html';
                }
            }
            $scope.DeleteMessages = () => {
                this.$userService.DeleteMessages(
                    $scope.DeletedIds,
                    () => {
                        $scope.DeletedIds.forEach((id) => {
                            var index = $scope.CurentUserMessages.map((message) => { return message.Id }).indexOf(id);
                            $scope.CurentUserMessages.splice(index, 1);
                        });

                    },
                    (reason) => { console.log(reason); });
            };
            $scope.AddPopId = (flag, Id) => {
                if (!$scope.DeletedIds)
                    $scope.DeletedIds = new Array<number>();
                if (flag == true) {
                    $scope.DeletedIds.push(Id);
                }
                if (flag == false) {
                    angular.forEach($scope.DeletedIds, (deleteId) => {
                        if (deleteId == Id) {
                            $scope.DeletedIds.splice($scope.DeletedIds.indexOf(deleteId), 1);
                        }
                    })
                }
            };



            $scope.MessageRead = (id: number) => {
                var message = $scope.CurentUserMessages.filter((message: Message) => {
                    return message.Id == id;
                })[0];
                message.State = true;
                this.$userService.PutMessage(
                    message,
                    (message: Message) => { },
                    (reason) => { console.log(reason); });
            };

            $scope.GetCustomPegeFriend = (query: string) => {
                if (this.$socialAccount == null)
                    this.$socialAccount = SocialAccount.GetAccount();
                this.$socialAccount.GetCustom(query).done((friends: SocialFriends) => {
                    $scope.$apply(() => { $scope.CurentUserFriends = friends; });
                })
            };

            $scope.SendMessage = (flag: boolean, id: number) => {
                var message = $scope.CurentUserMessages.filter((message: Message) => {
                    return message.Id == id;
                })[0];
                var tmp = new Message();
                tmp.Type = MessageType.Inner;
                tmp.UserId = message.MessengerId;
                tmp.UserEmail = message.MessengerEmail;
                tmp.MessengerId = message.UserId;
                tmp.MessengerEmail = message.UserEmail;


                if (flag == true) {
                    

                    message.Type = MessageType.Accept;
                    this.$userService.PutMessage(
                        message,
                        (message: Message) => { },
                        (reason) => { console.log(reason); });
                   

                    this.$travelService.PutTraveler(
                        message.TravelId, message.UserId,
                        () => { },
                        (error) => { console.log(error) }
                        );

                    tmp.Theme = "Підтвердження на поїздку";
                    tmp.MessageText = "Я беру вас в поїздку із задоволенням !";
                    

                    this.$userService.PostMessage(
                        tmp,
                        (message: Message) => { },
                        (reason) => { console.log(reason); });

                }
                if (flag == false) {
                    
                    message.Type = MessageType.Decline;
                    this.$userService.PutMessage(
                        message,
                        (message: Message) => { },
                        (reason) => { console.log(reason); });
                  

                    tmp.Theme = "Відмова на поїздку";
                    tmp.MessageText = "Вибачте, Ваше прохання про поїздку відхилине.";

                    this.$userService.PostMessage(
                        tmp,
                        (message: Message) => { },
                        (reason) => { console.log(reason); });
                }
            };

            $scope.GetDateString = (date: Date) => {return moment(date).format('DD MM  YYYY') };
            $scope.GetTimeString = (time: Date) => {return moment(time).format('hh:mm') };

            $scope.GetCommentType = (comment: CommentType) => {
                switch (comment) {
                    case CommentType.Positive: return "Позитивний"; break;
                    case CommentType.Neutral: return "Нейтральний"; break;
                    case CommentType.Negative: return "Негативний"; break;
                }
            };
        }
        SetupListener($scope, $rootScope, $resource, $location, $routeParams) {




        }
    }

    CabinetController['$inject'] = ['$scope', '$rootScope', '$resource', '$location', '$routeParams'];
}   