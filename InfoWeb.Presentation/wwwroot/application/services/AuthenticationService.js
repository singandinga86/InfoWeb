var module = angular.module("InfoWeb");

module.factory("AuthenticationService", ['$http', '$q', 'UrlService', function ($http, $q, UrlService) {

    var currentUser = "singandinga";

    return {
        login: function (userName, password) {
            var deferred = $q.defer();

            console.log(userName + " " + password);

            $http({
                method: "POST",
                url: UrlService.getServiceRootUrl() + "api/User/Login",
                data: {
                    Name: userName,
                    Password: password
                }
            }).then(function (response) {
                currentUser = response.data;
                deferred.resolve(response);
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