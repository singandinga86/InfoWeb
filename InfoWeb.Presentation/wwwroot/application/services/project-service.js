var module = angular.module("InfoWeb");

module.factory("ProjectService", ['$http', '$q', 'UrlService', function ($http, $q, UrlService) {
    return {
        getProjectsForUser: function (user) {
            var userId = user.id;
            var url = UrlService.getApiUrlPrefix() + "user/" + userId + "/projects";

            return $http({
                method: 'GET',
                url: url
            });
        }
    }   
}]);