var app = angular.module("InfoWeb");

app.factory("RoleService", ["$http", "UrlService", function ($http, UrlService) {
    
    return {
        getRoles: function () {
            return $http({
                method: 'GET',
                url: UrlService.getApiUrlPrefix() + "roles"
            });
        },
        getSearchRoles: function (searchValue) {
            return $http({
                method: 'GET',
                url: UrlService.getApiUrlPrefix() + "roles/search/" + searchValue
            });
        },
        getRole: function (id) {
            return $http({
                method: 'GET',
                url: UrlService.getApiUrlPrefix() + "roles/" + id
            });
        },
        createRole: function (role)
        {
            return $http({
                method: 'POST',
                url: UrlService.getApiUrlPrefix() + "roles",
                data: role
            });
        },
        updateRole: function (role) {
            return $http({
                method: 'PUT',
                url: UrlService.getApiUrlPrefix() + "roles",
                data: role
            });
        },
        removeRole: function (id) {
            return $http({
                method: 'DELETE',
                url: UrlService.getApiUrlPrefix() + "roles/" + id
            });
        }
    }
}]);