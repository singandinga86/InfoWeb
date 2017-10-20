var app = angular.module("InfoWeb");

app.controller("UpdateClientController", ['$scope', '$state', '$stateParams', 'ClientService', function ($scope, $state, $stateParams, ClientService) {

    var id = $stateParams.id;
    $scope.model = {};

    $scope.acceptButtonCaption = "Actualizar";
    $scope.title = "Actualizar Nombre";

    ClientService.getClient(id).then(function (response) {
        $scope.model = response.data;
    }, function (error) { });

    $scope.onButtonClicked = function () {
        ClientService.update($scope.model).then(function (response) {
            $state.go("clientList");
        }, function (error) { });
    }
}]);