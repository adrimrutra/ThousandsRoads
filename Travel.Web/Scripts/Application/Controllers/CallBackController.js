/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var CallBackController = (function () {
        function CallBackController($scope, $resource) {
            if (this.$userService == null)
                this.$userService = new Application.UserService($resource);
            $scope.MailSentForm = $('#mailsent').modal('hide');
            this.SetupScope($scope, $resource);
        }
        CallBackController.prototype.SetupScope = function ($scope, $resource) {
            var _this = this;
            $scope.SendMessage = function () {
                if ($scope.CurentMessage.MessageText) {
                    if ($scope.CurentMessage.MessageText && $scope.CurentMessage.MessageText.match(/\w/)) {
                        $scope.CurentMessage.Theme = "Поради по проекту.";
                        $scope.CurentMessage.UserEmail = "arturfedash@gmail.com";
                        _this.$userService.PostCallBack($scope.CurentMessage, function (flag) {
                            if (flag) {
                                $scope.MailSentForm.modal('show');
                                $scope.CurentMessage.MessengerEmail = "";
                                $scope.CurentMessage.MessengerDisplayName = "";
                                $scope.CurentMessage.MessageText = "";
                            }
                        }, function (reason) { console.log(reason); });
                    }
                }
            };
        };
        CallBackController.$viewtemplate = "/Content/Templates/CallBack.html";
        return CallBackController;
    })();
    Application.CallBackController = CallBackController;
    CallBackController['$inject'] = ['$scope', '$resource'];
})(Application || (Application = {}));
//# sourceMappingURL=CallBackController.js.map