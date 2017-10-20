var app = angular.module("InfoWeb");

app.factory("ProjectService", ['$http', 'UrlService',function ($http, UrlService) {
    return {
        getProjectDetails: function (userId, projectId)
        {
            return $http({
                method: 'GET',
                url: UrlService.getApiUrlPrefix() + userId + "/projects/" + projectId + "/details"
            });
        },
        getProjectsForUser: function (userId)
        {
            return $http({
                method: 'GET', 
                url: UrlService.getApiUrlPrefix() + userId + "/projects/"
            });
        },
        getUnassignedProjects: function (userId) {
            return $http({
                method: 'GET',
                url: UrlService.getApiUrlPrefix() + userId + "/projects/getUnassignedProjects"
            });
        },

    }
}]);