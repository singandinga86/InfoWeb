var app = angular.module("InfoWeb");

app.controller("HourTypesController", ['$scope', '$state', '$uibModal', 'HourTypeService', '$filter', 'NgTableParams','ngToast',
    function ($scope, $state, $uibModal, HourTypeService, $filter, NgTableParams, ngToast) {



    var fillTable = function ()
    {
        $scope.model = {};
        $scope.search = "";
        var orderedData = [];
       // HourTypeService.getHourTypes().then(function (response) {
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

                        return HourTypeService.getSearchHourType($scope.search).then(function (response) {
                            orderedData = $filter('orderBy')(response.data, params.orderBy());

                            params.total(orderedData.length);
                            return orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        }, function (error) {

                        });
                    }
                }
            );
            $scope.$watch("search", function () {
                console.log('asd');
                $scope.model.tableParams.reload();

            });       
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
           
        });
    }

}]);