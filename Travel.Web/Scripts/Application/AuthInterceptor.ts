/// <reference path="../Application/_references.ts"/>
module Application {
    'use strict';  
    var XAuthorization = "Authorization";
    var myModule = angular.module('AuthInterceptor', ['AuthInterceptorBuffer'])
        .directive('authInterceptor', ['$rootScope', '$injector', 'httpBuffer', '$cookieStore',
            ($rootScope, $injector: ng.auto.IInjectorService, httpBuffer, $cookieStore: ng.cookies.ICookieStoreService) => {
                return {
                    link: ($scope, elem, attrs) => {
                        var Login = (configUpdater: AuthHeader) => {
                            var updater = (config) => { return config; };
                            var $http = $injector.get('$http');
                            var $cookies = $injector.get('$cookieStore');

                            $cookies.remove(XAuthorization);
                            $cookies.put(XAuthorization, configUpdater.scheme + " " + configUpdater.param);

                            delete $http.defaults.headers.common[XAuthorization];
                            $http.defaults.headers.common[XAuthorization] = configUpdater.scheme + " " + configUpdater.param;

                            httpBuffer.retryAll(updater);
                        };
                        var Logout = () => {
                            var $http = $injector.get('$http');
                            var $cookies = $injector.get('$cookieStore');
                            $cookies.remove(XAuthorization);
                            delete $http.defaults.headers.common[XAuthorization];
                        };
                        $scope.$on('event:auth-logout', () => {
                            Logout();
                        });
                        $scope.$on('event:auth-login', (data, configUpdater: AuthHeader) => {
                            Login(configUpdater);
                        });
                    }
                }
            }])
        .config(['$httpProvider', '$injector', ($httpProvider, $injector) => {
            var interceptor = ['$rootScope', '$q', '$injector', 'httpBuffer', '$cookieStore', ($rootScope, $q, $injector, httpBuffer, $cookieStore) => {
                var success = (response) => {
                    return response;
                }
                var form = $("#interseptorauthorization").modal('hide'); 
                var error = (response) => {
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
                }

                return (promise) => {
                    return promise.then(success, error);
                };
            }];
            $httpProvider.responseInterceptors.push(interceptor);
        }]);

    angular.module('AuthInterceptorBuffer', [])
        .factory('httpBuffer', ['$injector', ($injector) => {
            var buffer = [];
            var $http;

            var retryHttpRequest = (config, deferred) => {
                var successCallback = (response) => {
                    deferred.resolve(response);
                }
                var errorCallback = (response) => {
                    deferred.reject(response);
                }
                $http = $http || $injector.get('$http');
                $http(config).then(successCallback, errorCallback);
            }

            return {
                append: (config, deferred) => {
                    delete config.headers[XAuthorization];
                    buffer.push({
                        config: config,
                        deferred: deferred
                    });
                },
                rejectAll: (reason) => {
                    if (reason) {
                        for (var i = 0; i < buffer.length; ++i) {
                            buffer[i].deferred.reject(reason);
                        }
                    }
                    buffer = [];
                },
                retryAll: (updater) => {
                    for (var i = 0; i < buffer.length; ++i) {
                        retryHttpRequest(updater(buffer[i].config), buffer[i].deferred);
                    }
                    buffer = [];
                }
            };
        }]);
}