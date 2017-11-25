var app = angular.module("InfoWeb");

app.controller("CreateUserController", ["$scope", '$state', "UserListService", 'RoleService', 'ngToast', function ($scope, $state, UserListService, RoleService, ngToast) {
    $scope.model = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear usuario";

    RoleService.getRoles().then(function (response) {
        $scope.roles = response.data;
    }, function (error) { });
    

    $scope.onButtonClicked = function () {
        if ($scope.model.password == $scope.model.passwordConfirmation) {
            UserListService.create($scope.model).then(function (response) {
                $state.go("userList");
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
    }
}]);