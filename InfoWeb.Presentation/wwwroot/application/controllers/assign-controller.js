var module = angular.module("InfoWeb");

app.controller("ProjectAssignController", ['$scope', 'ProjectService', 'AuthenticationService', 'UserListService', function ($scope, ProjectService, AuthenticationService, UserListService) {


    ProjectService.getProjectsForUser(AuthenticationService.getCurrentUser()).then(function (response) {

        $scope.projects = response.data;       
    },
        function (error) {

        });

    UserListService.getUser().then(function (response) {
        
        $scope.users = response.data;

    }, function (error) {

        });
 

    $scope.assignProject = function () {

        console.log($scope.selectedProject.id, $scope.selectedUser.id);

    };

}]);