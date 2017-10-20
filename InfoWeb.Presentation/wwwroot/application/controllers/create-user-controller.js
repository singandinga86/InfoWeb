var app = angular.module("InfoWeb");

app.controller("CreateUserController", ["$scope", '$state', "UserListService", function ($scope, $state, UserListService) {
    $scope.model = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear Usuario";

    $scope.onButtonClicked = function () {
        UserListService.create($scope.user).then(function (response) {
            $state.go("userList");
        }, function (error) {

        });
    }
}]);