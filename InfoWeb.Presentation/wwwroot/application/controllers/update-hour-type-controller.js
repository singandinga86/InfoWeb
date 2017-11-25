var app = angular.module("InfoWeb");

app.controller("UpdateHourTypeController", ['$scope', '$state', '$stateParams', 'HourTypeService', 'ngToast', function ($scope, $state, $stateParams, HourTypeService, ngToast) {

    var id = $stateParams.id;
    $scope.model = {};

    $scope.acceptButtonCaption = "Actualizar";
    $scope.title = "Actualizar Tipo de Hora";

    HourTypeService.getHourType(id).then(function (response) {
        $scope.model = response.data;
    }, function (error) { });

    $scope.onButtonClicked = function () {
        HourTypeService.update($scope.model).then(function (response) {
            $state.go("hourTypeList");
            ngToast.create({
                dismissButton: true,
                content: response.data
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