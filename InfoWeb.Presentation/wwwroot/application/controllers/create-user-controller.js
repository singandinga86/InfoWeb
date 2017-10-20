var app = angular.module("InfoWeb");

app.controller("CreateUserController", ["$scope", '$state', "UserListService", 'RoleService', function ($scope, $state, UserListService, RoleService) {
    $scope.model = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear Usuario";

    RoleService.getRoles().then(function (response) {
        $scope.roles = response.data;
    }, function (error) { });
    

    $scope.onButtonClicked = function () {
        if ($scope.model.password == $scope.model.passwordConfirmation) {
            UserListService.create($scope.model).then(function (response) {
                $state.go("userList");
            }, function (error) {

            });
        }
    }
}]);