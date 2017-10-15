var module = angular.module("InfoWeb");

module.factory("ProjectService", ['$http', '$q', 'UrlService', function ($http, $q, UrlService) {
    return {
        getAssignmentsForUser: function (user) {
            var userId = user.id;
            var url = UrlService.getApiUrlPrefix() + "user/" + userId + "/Assignments";

            return $http({
                method: 'GET',
                url: url
            });
        },
        getAssigmentType: function () {           
            var url = UrlService.getApiUrlPrefix() + "manager/assigmentType";

            return $http({
                method: 'GET',
                url: url
            });
        },
        getHoursType: function () {
            var url = UrlService.getApiUrlPrefix() + "manager/hourType";

            return $http({
                method: 'GET',
                url: url
            });
        }
    }   
}]);