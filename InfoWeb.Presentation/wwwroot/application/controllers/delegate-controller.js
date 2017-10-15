var module = angular.module("InfoWeb");

app.controller("ProjectDelegateController", ['$scope', 'ProjectService', 'AuthenticationService', 'UserListService', function ($scope, ProjectService, AuthenticationService, UserListService) {

    $scope.showHoras = false;
    $scope.assignment = {};

    ProjectService.getProjectsForUser(AuthenticationService.getCurrentUser()).then(function (response) {

        $scope.projects = response.data;       
    },
        function (error) {

        });

    UserListService.getUserAdmin().then(function (response) {

        $scope.users = response.data;

    }, function (error) {

        });

    ProjectService.getAssigmentType().then(function (response) {

        $scope.assigmentTypes = response.data;
    },
        function (error) {

        });

    ProjectService.getHoursType().then(function (response) {

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

        console.log($scope.assignment);

        ProjectService.createAssigment($scope.assignment).then(function (response) {
            console.log("pincha");
        }, function (error) {
            console.log("no pincha");
        });

    };



}]);