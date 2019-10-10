/// <reference path="_references.ts" />
var Application;
(function (Application) {
    angular.module("Loading", [])
        .config(function ($httpProvider) {
        $httpProvider.interceptors.push(["$q", "$injector", function ($q, $injector) {
                var $loader = $("#loading , #overlayer").hide();
                var $http;
                var $index;
                return {
                    request: function (request) {
                        $loader.show();
                        return request || $q.when(request);
                    },
                    response: function (response) {
                        $http = $http || $injector.get("$http");
                        if ($http.pendingRequests.length == 0) {
                            $loader.fadeOut();
                        }
                        return response || $q.when(response);
                    }
                };
            }]);
    });
})(Application || (Application = {}));
//# sourceMappingURL=Loading.js.map