var app = angular.module("InfoWeb");

app.controller("CreateRoleController", ["$scope", '$state', "RoleService", 'ngToast', function ($scope, $state, RoleService, ngToast) {
    $scope.role = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear Rol";

    $scope.onButtonClicked = function ()
    {
        RoleService.createRole($scope.role).then(function (response) {
            $state.go("roleList");
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