var app = angular.module("InfoWeb");

app.controller("CreateUserController", ["$scope", '$state', "UserListService", function ($scope, $state, UserListService) {
    $scope.role = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear Usuario";

    $scope.onButtonClicked = function () {
        RoleService.createRole($scope.role).then(function (response) {
            $state.go("userList");
        }, function (error) {

        });
    }
}]);