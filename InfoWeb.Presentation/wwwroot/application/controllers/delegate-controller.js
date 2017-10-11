var module = angular.module("InfoWeb");

app.controller("ProjectDelegateController", ['$scope', 'ProjectService', 'AuthenticationService', 'UserListService', function ($scope, ProjectService, AuthenticationService, UserListService) {

    $scope.showHoras = false;

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

        if ($scope.selectedTypeAssignment.name == "Asignar horas")
            $scope.showHoras = true;
        else
            $scope.showHoras = false;
    };

    $scope.send = function () {

        console.log($scope.selectedTypeAssignment.name);

    };



}]);