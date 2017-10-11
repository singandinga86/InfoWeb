var app = angular.module("InfoWeb");

app.controller("RoleListController", ['$scope', '$state', '$uibModal','RoleService', 'NgTableParams', function ($scope, $state, $uibModal, RoleService, NgTableParams) {

    $scope.search = { term: '' };
    $scope.model = {};

    var fillTable = function ()
    {
        RoleService.getRoles().then(function (response) {
            $scope.model.tableParams = new NgTableParams({
                page: 1,
                count: 6,
                filter: $scope.search
            }, {
                    total: response.data.length,
                    getData: function (params) {

                        if (params.filter().term) {
                            orderedData = params.filter() ? $filter('filter')(response.data, params.filter().term) : response.data;
                        } else {
                            orderedData = response.data;
                        }

                        //$defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));

                        params.total(orderedData.length);
                        return orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count());
                    }
                }
            );
        }, function (error) { });
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
            controller: "RemoveRoleDialogController"
        });

        dialog.result.then(function (result) {
            if (result == true)
            {
                RoleService.removeRole(role).then(function (response) {
                    fillTable();
                }, function (error) { });

            }
        }, function () {
            
        });


    }

}]);