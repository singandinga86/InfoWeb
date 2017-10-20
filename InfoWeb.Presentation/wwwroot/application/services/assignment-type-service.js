var app = angular.module("InfoWeb");

app.factory("AssignmentTypeService", ['$http', 'UrlService', function ($http, UrlService) {
    return {
        getAssignmentTypes: function () {
            var url = UrlService.getApiUrlPrefix() + 'AssignmentTypes';
            return $http({
                method: 'GET',
                url: url
            });
        },
        getAssignmentType: function (id) {
            var url = UrlService.getApiUrlPrefix() + 'AssignmentTypes/' + id;
            return $http({
                method: 'GET',
                url: url
            });
        },
        create: function (hourType) {
            var url = UrlService.getApiUrlPrefix() + 'AssignmentTypes';
            return $http({
                method: 'POST',
                url: url,
                data: hourType
            });
        },
        update: function (hourType) {
            var url = UrlService.getApiUrlPrefix() + 'AssignmentTypes';
            return $http({
                method: 'PUT',
                url: url,
                data: hourType
            });
        },
        remove: function (id) {
            var url = UrlService.getApiUrlPrefix() + 'AssignmentTypes/' + id;
            return $http({
                method: 'DELETE',
                url: url,
                data: id
            });
        }
    }
}]);