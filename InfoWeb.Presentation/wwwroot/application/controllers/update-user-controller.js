var app = angular.module("InfoWeb");

app.controller("UpdateUserController", ['$scope', '$state', '$stateParams', 'UserListService', function ($scope, $state, $stateParams, UserListService) {

    var id = $stateParams.id;
    $scope.model = {};

    $scope.acceptButtonCaption = "Actualizar";
    $scope.title = "Actualizar Nombre";

    UserListService.getUser(id).then(function (response) {
        $scope.model = response.data;
    }, function (error) { });

    $scope.onButtonClicked = function () {
        UserListService.update($scope.model).then(function (response) {
            $state.go("userList");
        }, function (error) { });
    }
}]);