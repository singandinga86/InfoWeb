var app = angular.module("InfoWeb");

app.controller("UpdateClientController", ['$scope', '$state', '$stateParams', 'ClientService', 'ngToast', function ($scope, $state, $stateParams, ClientService, ngToast) {

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