var module = angular.module("InfoWeb");

app.controller("ProjectAssignController", ['$scope', '$state', 'ProjectService', 'AuthenticationService', 'UserListService', 'AssignmentService',
    function ($scope, $state, ProjectService, AuthenticationService, UserListService, AssignmentService) {

    $scope.model = {}

    ProjectService.getUnassignedProjects(AuthenticationService.getCurrentUser().id).then(function (response) {

        $scope.projects = response.data;       
    },
        function (error) {

        });

    UserListService.getOMUsers().then(function (response) {
        
        $scope.users = response.data;

    }, function (error) {

        });
 

    $scope.assignProject = function () {
        AssignmentService.createProjectAssignment($scope.model).then(function (response) {
            $state.go('projectList');
        }, function (error) { });
    };

}]);