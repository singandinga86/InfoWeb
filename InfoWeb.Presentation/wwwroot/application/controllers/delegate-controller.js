var module = angular.module("InfoWeb");

app.controller("ProjectDelegateController",
    ['$scope', 'ProjectService', 'AssignmentService', 'AuthenticationService', 'UserListService',
        function ($scope, ProjectService, AssignmentService, AuthenticationService, UserListService) {

    $scope.showHoras = false;
    $scope.assignment = {};
    //$scope.assignment.hours = 35;
    $scope.project = {};

    ProjectService.getProjectsForUser(AuthenticationService.getCurrentUser().id).then(function (response) {

        $scope.projects = response.data;
    },
        function (error) {

        });

    UserListService.getUserAdmin().then(function (response) {

        $scope.users = response.data;

    }, function (error) {

        });

    AssignmentService.getAssigmentType().then(function (response) {

        $scope.assigmentTypes = response.data;
    },
        function (error) {

        });

    AssignmentService.getHoursType().then(function (response) {

        $scope.HoursTypes = response.data;       
    },
        function (error) {

        });

    $scope.changeValue = function () {

        if ($scope.selectedItem == true)
            $scope.showHoras = true;
        else
            $scope.showHoras = false;
    };

    $scope.send = function () {
        console.log($scope.assignment.project.project);
        $scope.assignment.project =  $scope.assignment.project;
        AssignmentService.createAssigment($scope.assignment).then(function (response) {           
            $scope.assignment = {};
            $scope.showHoras = false;
        }, function (error) {           
        });

    };

    //$scope.changeValue = function () {
    //    $scope.assignment.hours = 5;
    //};



}]);