var module = angular.module("InfoWeb");

module.factory("AuthenticationService", ['$http', '$q', '$window', 'UrlService', function ($http, $q, $window, UrlService) {

    var currentUser = null;

    return {
        login: function (userName, password) {
            var deferred = $q.defer();

            $http({
                method: "POST",
                url: UrlService.getServiceRootUrl() + "api/User/Login",
                data: {
                    Name: userName,
                    Password: password
                }
            }).then(function (response) {
                currentUser = response.data;
                $window.sessionStorage.setItem("user", JSON.stringify(currentUser));
                deferred.resolve(response);
            },
            function (error) {
                deferred.reject(error);
            });
            return deferred.promise;
        },

        getCurrentUser: function ()
        {
            if (currentUser == null)
            {
                $userString = $window.sessionStorage.getItem("user");
                $user = JSON.parse($userString);

                if ($user && $user.name && $user.role.name)
                {
                    currentUser = $user;
                }
            }
            return currentUser;
        },

        logout: function ()
        {
            $window.sessionStorage.removeItem("user");
        }
    }
}]);