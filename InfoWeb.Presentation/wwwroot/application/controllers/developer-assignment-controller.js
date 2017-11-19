var app = angular.module("InfoWeb");

app.controller("DeveloperAssignmentController",["$scope", '$state','AuthenticationService',
    'ProjectService', 'AssignmentService', 'UserListService','ngToast',
                function ($scope, $state, AuthenticationService, ProjectService,
                    AssignmentService, UserListService, ngToast) {

                   var assignator = AuthenticationService.getCurrentUser();
                   
                   ProjectService.getProjectsForUser(assignator.id).then(function (response) {
                       $scope.projects = response.data;                       
                   }, function (error) { });

                   UserListService.getTechnicians().then(function (response) {
                       $scope.users = response.data;                       
                       $scope.assignment.user = response.data[0].value;
                   }, function (error) { });

                   AssignmentService.getHoursType().then(function (response) {
                       $scope.hourTypes = response.data;
                   }, function (error) { });

                   $scope.assignment = {
                       user: assignator                     
                   };

                   $scope.create = function ()
                   {

                       AssignmentService.createTechnicianAssignment($scope.assignment).then(function (response) {
                           $state.go("projectList");
                           ngToast.create({
                               dismissButton: true,
                               content: 'El proyecto fue asignado satisfactoriamente.'
                           });
                       }, function (error) {
                           ngToast.create({
                               className: "danger",
                               dismissButton: true,
                               content: error.data.messages[0]
                           });
                       });
                   }




}]);