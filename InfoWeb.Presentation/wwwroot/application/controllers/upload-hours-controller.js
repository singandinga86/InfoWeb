﻿var module = angular.module("InfoWeb");

app.controller("UploadHoursController", ['$scope', '$state', 'ProjectService', 'AuthenticationService', 'UserListService', 'AssignmentService', 'ngToast',
    function ($scope, $state, ProjectService, AuthenticationService, UserListService, AssignmentService, ngToast) {

        $scope.model = {}    
        $scope.acceptButtonCaption = "Subir";
        $scope.title = "Subir Horas";

        ProjectService.getProjectsForUser(AuthenticationService.getCurrentUser().id).then(function (response) {

            $scope.projects = response.data;
        },
            function (error) {

            });
        $scope.onChangedProject = function () {
            if ($scope.model.project) {
                var projectSelected = $scope.model.project;
                var userId = AuthenticationService.getCurrentUser().id;
                AssignmentService.getHoursTypeByAssignment(projectSelected.id, userId).then(function (response) {
                    $scope.hourTypes = response.data;
                }, function (error) { });
            }
            else
                $scope.model.hourType = null;
        };

        $scope.today = function () {
            alert('asdasd');
        }

        $scope.uploadHours = function () {

            //AssignmentService.createProjectAssignmentGroup($scope.model).then(function (response) {
            //    $scope.model = {}
            //    $scope.model.usersSelected = [];
            //    $scope.userDuplicate = false;
            //    ngToast.create({
            //        dismissButton: true,
            //        content: response.data
            //    });
            //}, function (error) {
            //    ngToast.create({
            //        className: "danger",
            //        dismissButton: true,
            //        content: error.data.messages[0]
            //    });
            //});
        };

    }]);