var module = angular.module("InfoWeb");

module.factory("UserListService", ['$http', '$q', 'UrlService', function ($http, $q, UrlService) {
    return {
        getUser: function () {            
            var url = UrlService.getApiUrlPrefix() + "User/list";

            return $http({
                method: 'GET',
                url: url
            });
        },
        getUserAdmin: function () {
            var url = UrlService.getApiUrlPrefix() + "User/listAdmin";

            return $http({
                method: 'GET',
                url: url
            });
        },
        getTechnicians: function () {
            var url = UrlService.getApiUrlPrefix() + "User/listTechnicians";

            return $http({
                method: 'GET',
                url: url
            }); 
        }
    }
}]);