var module = angular.module("InfoWeb");

module.factory("UserListService", ['$http', '$q', 'UrlService', function ($http, $q, UrlService) {
    return {
        getUser: function () {            
            var url = UrlService.getApiUrlPrefix() + "/User";

            return $http({
                method: 'GET',
                url: url
            });
        }
    }
}]);