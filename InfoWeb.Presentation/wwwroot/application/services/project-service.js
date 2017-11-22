var module = angular.module("InfoWeb");

module.factory("AssignmentService", ['$http', '$q','UrlService', 'AuthenticationService',
      function ($http, $q, UrlService, AuthenticationService) {
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
        },
        getHoursTypeByProject: function (idProject) {
            var url = UrlService.getApiUrlPrefix() + "manager/hourTypeByProject/" + idProject;

            return $http({
                method: 'GET',
                url: url
            });
        },
        createAssigment: function (assignment) {
            var userId = AuthenticationService.getCurrentUser().id;
            return $http({
                method: 'POST',
                url: UrlService.getApiUrlPrefix() + "user/" + userId + "/Assignments",
                data: assignment
            });
        },
        createTechnicianAssignment: function (assignment) {
            var userId = AuthenticationService.getCurrentUser().id;
            return $http({
                method: 'POST',
                url: UrlService.getApiUrlPrefix() + "user/" + userId + "/Assignments/technician",
                data: assignment
            });
        },
        createProjectAssignment: function (inputData) {
            var userId = AuthenticationService.getCurrentUser().id;
            return $http({
                method: 'POST',
                url: UrlService.getApiUrlPrefix() + "user/" + userId + "/Assignments/project",
                data: {
                    projectId: inputData.project.id,
                    assigneeId: inputData.assignee.id
                }
            });
        },
        createProjectAssignmentGroup: function (inputData) {
            var userId = AuthenticationService.getCurrentUser().id;
            return $http({
                method: 'POST',
                url: UrlService.getApiUrlPrefix() + "user/" + userId + "/Assignments/projectGroup",
                data: inputData
            });
        }
    }   
}]);