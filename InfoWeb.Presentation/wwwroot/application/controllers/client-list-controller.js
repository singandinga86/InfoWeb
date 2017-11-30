var app = angular.module("InfoWeb");

app.controller("ClientController", ['$scope', '$state', '$uibModal', 'ClientService', '$filter', 'NgTableParams', 'ngToast', function ($scope, $state, $uibModal, ClientService, $filter, NgTableParams, ngToast) {

    $scope.model = {};
    $scope.search = "";
    var orderedData = [];

    var fillTable = function () {
        //ClientService.getCliente().then(function (response) {
            $scope.model.tableParams = new NgTableParams({
                page: 1,
                count: 10,
               // filter: $scope.search
                sorting: {
                    name: 'asc'
                }
            }, {
                   // total: response.data.length,
                    getData: function (params) {

                        return ClientService.getSearchCliente($scope.search).then(function (response) {
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

    $scope.onCreateClicked = function () {
        $state.go("createClient");
    }

    $scope.onEditClicked = function (id) {
        $state.go('updateClient', { id: id })
    }

    $scope.onRemoveClicked = function (target) {
        var dialog = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'application/views/client/remove-modal.html',
            controller: "RemoveClientDialogController",
            resolve: {
                target: function () { return target; }
            }
        });

        dialog.result.then(function (result) {
            if (result == true) {
                ClientService.remove(target.id).then(function (response) {
                    fillTable();
                    $state.go("clientList");
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