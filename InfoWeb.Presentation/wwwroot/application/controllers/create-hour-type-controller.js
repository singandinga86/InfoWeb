var app = angular.module("InfoWeb");

app.controller("CreateHourTypeController", ["$scope", '$state', "HourTypeService", 'ngToast', function ($scope, $state, HourTypeService, ngToast) {
    $scope.model = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear tipo de hora";

    $scope.onButtonClicked = function () {
        HourTypeService.create($scope.model).then(function (response) {
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