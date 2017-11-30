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
        getSearchUser: function (searchValue) {
            var url = UrlService.getApiUrlPrefix() + "User/search/" + searchValue;

            return $http({
                method: 'GET',
                url: url
            });
        },
        getUserById: function (id)
        {
            var url = UrlService.getApiUrlPrefix() + "User/" + id;

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
        create: function (userModel) {
            var url = UrlService.getApiUrlPrefix() + 'User';
            return $http({
                method: 'POST',
                url: url,
                data: userModel
            });
        },
        update: function (userModel) {
            var url = UrlService.getApiUrlPrefix() + 'User';
            return $http({
                method: 'PUT',
                url: url,
                data: userModel
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