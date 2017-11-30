var app = angular.module("InfoWeb");

app.factory("ProjectTypesService", ['$http', 'UrlService', function ($http, UrlService) {
    return {
        getProjectTypes: function () {
            var url = UrlService.getApiUrlPrefix() + 'ProjectTypes';
            return $http({
                method: 'GET',
                url: url
            });
        },
        getSearchProjectTypes: function (searchValue) {
            var url = UrlService.getApiUrlPrefix() + 'ProjectTypes/search/' + searchValue;
            return $http({
                method: 'GET',
                url: url
            });
        },
        getProjectType: function (id) {
            var url = UrlService.getApiUrlPrefix() + 'ProjectTypes/' + id;
            return $http({
                method: 'GET',
                url: url
            });
        },
        create: function (projectType) {
            var url = UrlService.getApiUrlPrefix() + 'ProjectTypes';
            return $http({
                method: 'POST',
                url: url,
                data: projectType
            });
        },
        update: function (projectType) {
            var url = UrlService.getApiUrlPrefix() + 'ProjectTypes';
            return $http({
                method: 'PUT',
                url: url,
                data: projectType
            });
        },
        remove: function (id) {
            var url = UrlService.getApiUrlPrefix() + 'ProjectTypes/' + id;
            return $http({
                method: 'DELETE',
                url: url,
                data: id
            });
        }
    }
}]);