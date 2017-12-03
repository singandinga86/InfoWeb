var app = angular.module("InfoWeb");

app.factory("ProjectService", ['$http', 'UrlService',function ($http, UrlService) {
    return {
        getProjectDetails: function (userId, projectId) {
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
        removeProject: function (userId, projectId) {
            return $http({
                method: 'DELETE',
                url: UrlService.getApiUrlPrefix() + userId + "/projects/" + projectId
            });
        },
        searchProjects: function (userId, searchValue) {
            return $http({
                mehtod: 'GET',
                url: UrlService.getApiUrlPrefix() + userId + "/projects/search/" + searchValue
            });
        },
        create: function (userId, project) {
            var url = UrlService.getApiUrlPrefix() + userId + '/projects';
            return $http({
                method: 'POST',
                url: url,
                data: project
            });
        },
        update: function (userId, project) {
            var url = UrlService.getApiUrlPrefix() + userId + '/projects';
            return $http({
                method: 'PUT',
                url: url,
                data: project
            });
        },
        getProject: function (projectId, userId) {
            return $http({
                method: 'GET',
                url: UrlService.getApiUrlPrefix() + userId + "/projects/" + projectId + "/getProject"
            });
        },
        canBeRemoved: function (projectId, userId)
        {
            return $http({
                method: 'GET',
                url: UrlService.getApiUrlPrefix() + userId + "/projects/canBeRemoved/" + projectId
            });
        }

    }
}]);