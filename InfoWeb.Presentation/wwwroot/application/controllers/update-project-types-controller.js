var app = angular.module("InfoWeb");

app.controller("UpdateProjectTypesController", ['$scope', '$state', '$stateParams', 'ProjectTypesService', function ($scope, $state, $stateParams, ProjectTypesService) {

    var id = $stateParams.id;
    $scope.model = {};

    $scope.acceptButtonCaption = "Actualizar";
    $scope.title = "Actualizar Tipo de Proyecto";

    ProjectTypesService.getProjectType(id).then(function (response) {
        $scope.model = response.data;
    }, function (error) { });

    $scope.onButtonClicked = function () {
        ProjectTypesService.update($scope.model).then(function (response) {
            $state.go("projectTypesList");
        }, function (error) { });
    }
}]);