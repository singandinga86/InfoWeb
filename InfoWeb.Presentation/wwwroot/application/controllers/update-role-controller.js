var app = angular.module("InfoWeb");

app.controller("UpdateRoleController", ['$scope', '$state', '$stateParams', 'RoleService', function ($scope, $state, $stateParams, RoleService) {

    var roleId = $stateParams.roleId;
    $scope.role = {};

    $scope.acceptButtonCaption = "Actualizar";
    $scope.title = "Actualizar Rol";

    RoleService.getRole(roleId).then(function (response) {
        $scope.role = response.data;
    }, function (error) { });

    $scope.onButtonClicked = function ()
    {
        RoleService.updateRole($scope.role).then(function (response) {
            $state.go("roleList");
        }, function (error) { });
    }
}]);