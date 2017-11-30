var app = angular.module("InfoWeb");

app.controller("RoleListController", ['$scope', '$state', '$filter', '$uibModal', 'RoleService', 'NgTableParams','ngToast',
    function ($scope, $state, $filter, $uibModal, RoleService, NgTableParams, ngToast) {

        $scope.search = "";
    $scope.model = {};

    var fillTable = function ()
    {
        //RoleService.getRoles().then(function (response) {
            $scope.model.tableParams = new NgTableParams({
                page: 1,
                count: 10,
                sorting: {
                    name: 'asc'
                }
               // filter: $scope.search
            }, {
                    //total: response.data.length,
                    getData: function (params) {

                        return RoleService.getSearchRoles($scope.search).then(function (response) {
                            orderedData = $filter('orderBy')(response.data, params.orderBy());

                            params.total(orderedData.length);
                            return orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        }, function (error) {

                        });
                    }
                }
            );
       // }, function (error) { });
            $scope.$watch("search", function () {

                $scope.model.tableParams.reload();

            });
    }

    fillTable();

    $scope.onCreateClicked = function ()
    {
        $state.go("createRole");
    }

    $scope.onEditClicked = function (id)
    {
        $state.go('updateRole', { roleId: id })
    }

    $scope.onRemoveClicked = function (role)
    {
        var dialog = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'application/views/role/remove-modal.html',
            controller: "RemoveRoleDialogController",
            resolve: {
                targetRole: function () { return role; }
            }
        });

        dialog.result.then(function (result) {
            if (result == true)
            {
                RoleService.removeRole(role.id).then(function (response) {
                    fillTable();
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
        }, function () {
           
        });


    }

}]);