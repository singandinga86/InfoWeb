var app = angular.module("InfoWeb");

app.controller("UpdateUserController", ['$q','$scope', '$state', '$stateParams', 'UserListService', 'RoleService',
    function ($q,$scope, $state, $stateParams, UserListService, RoleService) {

    var id = $stateParams.userId;
    $scope.model = {};

    $scope.acceptButtonCaption = "Actualizar";
    $scope.title = "Actualizar usuario";

    var userPromise = UserListService.getUserById(id);/*.then(function (response) {
        var data = response.data;
        $scope.model.id = data.id;
        $scope.model.name = data.name;
        $scope.model.role = data.role;
        $scope.model.password = data.password;
        $scope.model.passwordConfirmation = data.password;
        
    }, function (error) { });*/

    var rolePromise = RoleService.getRoles();/*.then(function (response) {
        $scope.roles = response.data;
    }, function (error) { });*/

    $q.all([userPromise, rolePromise]).then(function (response) {
        $scope.roles = response[1].data

        var userData = response[0].data;
        $scope.model.id = userData.id;
        $scope.model.name = userData.name;
        $scope.model.role = userData.role;
        $scope.model.password = userData.password;
        $scope.model.passwordConfirmation = userData.password;
    }, function (error) { });


    $scope.onButtonClicked = function () {
        UserListService.update($scope.model).then(function (response) {
            $state.go("userList");
        }, function (error) { });
    }
}]);