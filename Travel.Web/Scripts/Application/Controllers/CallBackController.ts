/// <reference path="../_references.ts" />
module Application {

    export interface ICallBackScope extends ng.IScope {

        SendMessage: Function;
        CurentMessage: Message;
        MailSentForm: JQuery;
    }
    export class CallBackController {
        public static $viewtemplate = "/Content/Templates/CallBack.html";
        private $userService: UserService;
        constructor($scope: ICallBackScope, $resource: IResourceService) {

            if (this.$userService == null)
                this.$userService = new UserService($resource);

            $scope.MailSentForm = $('#mailsent').modal('hide');
            this.SetupScope($scope, $resource);

        }


        SetupScope($scope: ICallBackScope, $resource: IResourceService) {

            $scope.SendMessage = () => {
                    
                if ($scope.CurentMessage.MessageText  ) {
                    if ($scope.CurentMessage.MessageText && $scope.CurentMessage.MessageText.match(/\w/)) {
                        $scope.CurentMessage.Theme = "Поради по проекту.";
                        $scope.CurentMessage.UserEmail = "arturfedash@gmail.com";

                        this.$userService.PostCallBack(
                            $scope.CurentMessage,
                            (flag: boolean) => {
                                if (flag) {
                                    $scope.MailSentForm.modal('show');
                                    $scope.CurentMessage.MessengerEmail = "";
                                    $scope.CurentMessage.MessengerDisplayName = "";
                                    $scope.CurentMessage.MessageText = "";
                                }
                            },
                            (reason) => { console.log(reason); });

                    }
                }
            }


        }
    }
    CallBackController['$inject'] = ['$scope', '$resource'];
} 