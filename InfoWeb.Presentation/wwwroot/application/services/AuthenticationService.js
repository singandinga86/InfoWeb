var module = angular.module("InfoWeb");

module.factory("AuthenticationService", ['$http', '$q', 'UrlService', function ($http, $q, UrlService) {

    var currentUser = null;

    return {
        login: function (userName, password) {
            var deferred = $q.defer();

            $http.post({
                url: UrlService.getServiceRootUrl() + "api/User/Login",
                data: {
                    Name: userName,
                    Password: password
                }
            }).then(function (data) {
                currentUser = data;
                deferred.resolve(data);
            },
            function (error) {
                deferred.reject(error);
            });
            return deferred.promise;
        },

        getCurrentUser: function ()
        {
            return currentUser;
        }
    }
}]);