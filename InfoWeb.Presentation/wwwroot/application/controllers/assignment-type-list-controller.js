var app = angular.module("InfoWeb");

app.controller("AssignmentTypeListController", ['$scope', '$state', '$uibModal', 'AssignmentTypeService', '$filter', 'NgTableParams', function ($scope, $state, $uibModal, AssignmentTypeService, $filter, NgTableParams) {

    $scope.model = {};
    $scope.search = { term: '' };
    var orderedData = [];


    var fillTable = function () {
        AssignmentTypeService.getAssignmentTypes().then(function (response) {
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
        $state.go("createAssignmentType");
    }

    $scope.onEditClicked = function (id) {
        $state.go('updateAssignmentType', { id: id })
    }

    $scope.onRemoveClicked = function (target) {
        var dialog = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'application/views/assignment-type/remove-modal.html',
            controller: "RemoveAssignmentTypeDialogController",
            resolve: {
                target: function () { return target; }
            }
        });

        dialog.result.then(function (result) {
            if (result == true) {
                AssignmentTypeService.remove(target.id).then(function (response) {
                    fillTable();
                }, function (error) { });

            }
        }, function () {

        });
    }

}]);