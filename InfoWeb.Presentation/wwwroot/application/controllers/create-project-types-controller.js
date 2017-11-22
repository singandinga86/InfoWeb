var app = angular.module("InfoWeb");

app.controller("CreateProjectTypesController", ["$scope", '$state', "ProjectTypesService","ngToast",
    function ($scope, $state, ProjectTypesService, ngToast) {
    $scope.model = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear tipo de proyecto";

    $scope.onButtonClicked = function () {
        ProjectTypesService.create($scope.model).then(function (response) {
            $state.go("projectTypesList");
            ngToast.create({
                dismissButton: true,
                content: 'El tipo de proyecto fue creado satisfactoriamente.'
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