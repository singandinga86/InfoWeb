var app = angular.module("InfoWeb");

app.controller("DeveloperAssignmentController",["$scope", '$state','AuthenticationService',
    'ProjectService', 'AssignmentService', 'UserListService',
                function ($scope, $state, AuthenticationService, ProjectService,
                    AssignmentService, UserListService) {

                   var assignator = AuthenticationService.getCurrentUser();
                   
                   ProjectService.getProjectsForUser(assignator.id).then(function (response) {
                       $scope.projects = response.data;
                   }, function (error) { });

                   UserListService.getTechnicians().then(function (response) {
                       $scope.technicians = response.data;
                   }, function (error) { });

                   AssignmentService.getHoursType().then(function (response) {
                       $scope.hourTypes = response.data;
                   }, function (error) { });

                   $scope.assignment = {
                       user: assignator,
                       hours : 20
                   };

                   $scope.create = function ()
                   {

                       AssignmentService.createTechnicianAssignment($scope.assignment).then(function (response) {
                           console.log("pincha");
                       }, function (error) {
                       });
                   }




}]);