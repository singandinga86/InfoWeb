var app = angular.module("InfoWeb");

app.controller("UpdateRoleController", ['$scope', '$state', '$stateParams', 'RoleService','ngToast',
    function ($scope, $state, $stateParams, RoleService, ngToast) {

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
            ngToast.create({
                dismissButton: true,
                content: 'El rol fue actualizado satisfactoriamente.'
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