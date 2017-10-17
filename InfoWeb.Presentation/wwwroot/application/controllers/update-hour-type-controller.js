var app = angular.module("InfoWeb");

app.controller("UpdateHourTypeController", ['$scope', '$state', '$stateParams', 'HourTypeService', function ($scope, $state, $stateParams, HourTypeService) {

    var id = $stateParams.id;
    $scope.model = {};

    $scope.acceptButtonCaption = "Actualizar";
    $scope.title = "Actualizar Rol";

    HourTypeService.getHourType(id).then(function (response) {
        $scope.model = response.data;
    }, function (error) { });

    $scope.onButtonClicked = function () {
        HourTypeService.update($scope.model).then(function (response) {
            $state.go("hourTypeList");
        }, function (error) { });
    }
}]);