var app = angular.module("InfoWeb");

app.factory("ClientService", ['$http', 'UrlService', function ($http, UrlService) {
    return {
        getCliente: function () {
            var url = UrlService.getApiUrlPrefix() + 'Client';
            return $http({
                method: 'GET',
                url: url
            });
        },
        getSearchCliente: function (searchValue) {
            var url = UrlService.getApiUrlPrefix() + 'Client/search/' + searchValue;
            return $http({
                method: 'GET',
                url: url
            });
        },
        getClient: function (id) {
            var url = UrlService.getApiUrlPrefix() + 'Client/' + id;
            return $http({
                method: 'GET',
                url: url
            });
        },
        create: function (client)
        {
            var url = UrlService.getApiUrlPrefix() + 'Client';
            return $http({
                method: 'POST',
                url: url,
                data: client
            });
        },
        update: function (client)
        {
            var url = UrlService.getApiUrlPrefix() + 'Client';
            return $http({
                method: 'PUT',
                url: url,
                data: client
            });
        },
        remove: function (id)
        {
            var url = UrlService.getApiUrlPrefix() + 'Client/' + id;
            return $http({
                method: 'DELETE',
                url: url,
                data: id
            });
        }
    }
}]);