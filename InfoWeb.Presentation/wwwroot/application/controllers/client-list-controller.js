var app = angular.module("InfoWeb");

app.controller("ClientController", ['$scope', '$state', '$uibModal', 'ClientService', 'NgTableParams', function ($scope, $state, $uibModal, ClientService, NgTableParams) {

    $scope.model = {};
    $scope.search = { term: '' };
    var orderedData = [];

    var fillTable = function () {
        ClientService.getCliente().then(function (response) {
            console.log(response);
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
                }, function (error) { });

            }
        }, function () {

        });
    }

}]);