var app = angular.module("InfoWeb");

app.controller("UpdateUserController", ['$scope', '$state', '$stateParams', 'UserListService', 'RoleService',
    function ($scope, $state, $stateParams, UserListService, RoleService) {

    var id = $stateParams.userId;
    $scope.model = {};

    $scope.acceptButtonCaption = "Actualizar";
    $scope.title = "Actualizar usuario";

    UserListService.getUserById(id).then(function (response) {
        var data = response.data;
        $scope.model.id = data.id;
        $scope.model.name = data.name;
        $scope.model.role = data.role;
        $scope.model.password = data.password;
        $scope.model.passwordConfirmation = data.password;
        
    }, function (error) { });

    RoleService.getRoles().then(function (response) {
        $scope.roles = response.data;
    }, function (error) { });


    $scope.onButtonClicked = function () {
        UserListService.update($scope.model).then(function (response) {
            $state.go("userList");
        }, function (error) { });
    }
}]);