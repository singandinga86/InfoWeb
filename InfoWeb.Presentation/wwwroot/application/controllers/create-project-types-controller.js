var app = angular.module("InfoWeb");

app.controller("CreateProjectTypesController", ["$scope", '$state', "ProjectTypesService", function ($scope, $state, ProjectTypesService) {
    $scope.model = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear tipo de proyecto";

    $scope.onButtonClicked = function () {
        ProjectTypesService.create($scope.model).then(function (response) {
            $state.go("projectTypesList");
        }, function (error) {

        });
    }
}]);