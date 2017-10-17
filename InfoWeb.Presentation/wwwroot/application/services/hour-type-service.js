var app = angular.module("InfoWeb");

app.factory("HourTypeService", ['$http','UrlService', function ($http, UrlService) {
    return {
        getHourTypes: function () {
            var url = UrlService.getApiUrlPrefix() + 'HourTypes';
            return $http({
                method: 'GET',
                url: url
            });
        },
        getHourType: function (id) {
            var url = UrlService.getApiUrlPrefix() + 'HourTypes/' + id;
            return $http({
                method: 'GET',
                url: url
            });
        },
        create: function (hourType)
        {
            var url = UrlService.getApiUrlPrefix() + 'HourTypes';
            return $http({
                method: 'POST',
                url: url,
                data: hourType
            });
        },
        update: function (hourType)
        {
            var url = UrlService.getApiUrlPrefix() + 'HourTypes';
            return $http({
                method: 'PUT',
                url: url,
                data: hourType
            });
        },
        remove: function (id)
        {
            var url = UrlService.getApiUrlPrefix() + 'HourTypes/' + id;
            return $http({
                method: 'DELETE',
                url: url,
                data: id
            });
        }
    }
}]);