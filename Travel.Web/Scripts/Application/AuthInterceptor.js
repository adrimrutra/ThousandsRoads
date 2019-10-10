/// <reference path="../Application/_references.ts"/>
var Application;
(function (Application) {
    'use strict';
    var XAuthorization = "Authorization";
    var myModule = angular.module('AuthInterceptor', ['AuthInterceptorBuffer'])
        .directive('authInterceptor', ['$rootScope', '$injector', 'httpBuffer', '$cookieStore',
        function ($rootScope, $injector, httpBuffer, $cookieStore) {
            return {
                link: function ($scope, elem, attrs) {
                    var Login = function (configUpdater) {
                        var updater = function (config) { return config; };
                        var $http = $injector.get('$http');
                        var $cookies = $injector.get('$cookieStore');
                        $cookies.remove(XAuthorization);
                        $cookies.put(XAuthorization, configUpdater.scheme + " " + configUpdater.param);
                        delete $http.defaults.headers.common[XAuthorization];
                        $http.defaults.headers.common[XAuthorization] = configUpdater.scheme + " " + configUpdater.param;
                        httpBuffer.retryAll(updater);
                    };
                    var Logout = function () {
                        var $http = $injector.get('$http');
                        var $cookies = $injector.get('$cookieStore');
                        $cookies.remove(XAuthorization);
                        delete $http.defaults.headers.common[XAuthorization];
                    };
                    $scope.$on('event:auth-logout', function () {
                        Logout();
                    });
                    $scope.$on('event:auth-login', function (data, configUpdater) {
                        Login(configUpdater);
                    });
                }
            };
        }])
        .config(['$httpProvider', '$injector', function ($httpProvider, $injector) {
            var interceptor = ['$rootScope', '$q', '$injector', 'httpBuffer', '$cookieStore', function ($rootScope, $q, $injector, httpBuffer, $cookieStore) {
                    var success = function (response) {
                        return response;
                    };
                    var form = $("#interseptorauthorization").modal('hide');
                    var error = function (response) {
                        if (response.status === 401 && !response.config.ignoreAuthModule) {
                            var deferred = $q.defer();
                            httpBuffer.append(response.config, deferred);
                            $rootScope.$broadcast('event:auth-logout');
                            //error message  
                            form.modal('show');
                            //  alert(401);
                            return deferred.promise;
                        }
                        return $q.reject(response);
                    };
                    return function (promise) {
                        return promise.then(success, error);
                    };
                }];
            $httpProvider.responseInterceptors.push(interceptor);
        }]);
    angular.module('AuthInterceptorBuffer', [])
        .factory('httpBuffer', ['$injector', function ($injector) {
            var buffer = [];
            var $http;
            var retryHttpRequest = function (config, deferred) {
                var successCallback = function (response) {
                    deferred.resolve(response);
                };
                var errorCallback = function (response) {
                    deferred.reject(response);
                };
                $http = $http || $injector.get('$http');
                $http(config).then(successCallback, errorCallback);
            };
            return {
                append: function (config, deferred) {
                    delete config.headers[XAuthorization];
                    buffer.push({
                        config: config,
                        deferred: deferred
                    });
                },
                rejectAll: function (reason) {
                    if (reason) {
                        for (var i = 0; i < buffer.length; ++i) {
                            buffer[i].deferred.reject(reason);
                        }
                    }
                    buffer = [];
                },
                retryAll: function (updater) {
                    for (var i = 0; i < buffer.length; ++i) {
                        retryHttpRequest(updater(buffer[i].config), buffer[i].deferred);
                    }
                    buffer = [];
                }
            };
        }]);
})(Application || (Application = {}));
//# sourceMappingURL=AuthInterceptor.js.map