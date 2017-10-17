var app = angular.module("InfoWeb");

app.controller("CreateHourTypeController", ["$scope", '$state', "HourTypeService", function ($scope, $state, HourTypeService) {
    $scope.model = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear tipo de hora";

    $scope.onButtonClicked = function () {
        HourTypeService.create($scope.model).then(function (response) {
            $state.go("hourTypeList");
        }, function (error) {

        });
    }
}]);