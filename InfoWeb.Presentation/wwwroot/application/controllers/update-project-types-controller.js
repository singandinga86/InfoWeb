var app = angular.module("InfoWeb");

app.controller("UpdateProjectTypesController", ['$scope', '$state', '$stateParams', 'ProjectTypesService',"ngToast",
    function ($scope, $state, $stateParams, ProjectTypesService, ngToast) {

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
            ngToast.create({
                dismissButton: true,
                content: 'El tipo de proyecto fue actualizado satisfactoriamente.'
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