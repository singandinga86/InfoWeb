var app = angular.module("InfoWeb");

app.controller("HourTypesController", ['$scope', '$state', '$uibModal', 'HourTypeService', '$filter', 'NgTableParams','ngToast',
    function ($scope, $state, $uibModal, HourTypeService, $filter, NgTableParams, ngToast) {

    $scope.model = {};
    $scope.search = { term: '' };
    var orderedData = [];

    var fillTable = function ()
    {
        HourTypeService.getHourTypes().then(function (response) {
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
        $state.go("createHourType");
    }

    $scope.onEditClicked = function (id) {
        $state.go('updateHourType', { id: id })
    }

    $scope.onRemoveClicked = function (target) {
        var dialog = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'application/views/hour-type/remove-modal.html',
            controller: "RemoveHourTypeDialogController",
            resolve: {
                target: function () { return target; }
            }
        });

        dialog.result.then(function (result) {
            if (result == true) {
                HourTypeService.remove(target.id).then(function (response) {
                    fillTable();
                    ngToast.create({
                        dismissButton: true,
                        content: 'El tipo de hora fue eliminado satisfactoriamente.'
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
            ngToast.create({
                className: "danger",
                dismissButton: true,
                content: "Ocurrióun error."
            });
        });
    }

}]);