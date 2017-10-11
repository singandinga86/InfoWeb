var app = angular.module("InfoWeb");

app.controller("CreateRoleController", ["$scope", '$state',"RoleService", function ($scope, $state, RoleService) {
    $scope.role = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear rol";

    $scope.onButtonClicked = function ()
    {
        RoleService.createRole($scope.role).then(function (response) {
            $state.go("roleList");
        }, function (error) {
           
        });
    }
}]);