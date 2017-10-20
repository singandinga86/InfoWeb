var app = angular.module("InfoWeb");

app.controller("CreateClientController", ["$scope", '$state', "ClientService", function ($scope, $state, ClientService) {
    $scope.model = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear cliente";

    $scope.onButtonClicked = function () {
        ClientService.create($scope.model).then(function (response) {
            $state.go("ClientList");
        }, function (error) {

            });
       
    }
}]);