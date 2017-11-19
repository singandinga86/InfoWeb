var app = angular.module("InfoWeb");

app.controller("CreateClientController", ["$scope", '$state', "ClientService", 'ngToast', function ($scope, $state, ClientService, ngToast) {
    $scope.model = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear cliente";

    $scope.onButtonClicked = function () {
        ClientService.create($scope.model).then(function (response) {
            $state.go("clientList");
            ngToast.create({
                dismissButton: true,
                content: 'El cliente fue creado satisfactoriamente.'
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