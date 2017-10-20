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
        getOMUsers: function () {            
            var url = UrlService.getApiUrlPrefix() + "User/listOpManagers";

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
        },
        create: function (user) {
            var url = UrlService.getApiUrlPrefix() + 'User';
            return $http({
                method: 'POST',
                url: url,
                data: user
            });
        },
        update: function (user) {
            var url = UrlService.getApiUrlPrefix() + 'User';
            return $http({
                method: 'PUT',
                url: url,
                data: user
            });
        },
        remove: function (id) {
            var url = UrlService.getApiUrlPrefix() + 'User/' + id;
            return $http({
                method: 'DELETE',
                url: url,
                data: id
            });
        }
    }
}]);