/// <reference path="_references.ts" />
module Application {

    angular.module("Loading", [])
        .config(($httpProvider: ng.IHttpProvider) => {
            $httpProvider.interceptors.push(["$q", "$injector", ($q: ng.IQService, $injector: ng.auto.IInjectorService) => {
                var $loader = $("#loading , #overlayer").hide();
                var $http: ng.IHttpService;
                var $index: number;
                return {
                    request: (request) => {
                        $loader.show();
                        return request || $q.when(request);
                    },
                    response: (response) => {
                        $http = $http || $injector.get("$http");
                        if ($http.pendingRequests.length == 0)
                        {
                            $loader.fadeOut();
                        }
                        return response || $q.when(response);
                    }    
                }
            }])
        });
}