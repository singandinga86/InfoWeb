﻿var app = angular.module("InfoWeb");

app.controller("AssignmentTypeListController", ['$scope', '$state', '$uibModal', 'AssignmentTypeService', '$filter', 'NgTableParams','ngToast',
    function ($scope, $state, $uibModal, AssignmentTypeService, $filter, NgTableParams, ngToast) {



        var fillTable = function () {

            $scope.model = {};
            $scope.search = "";
            var orderedData = [];
        //AssignmentTypeService.getAssignmentTypes().then(function (response) {
            $scope.model.tableParams = new NgTableParams({
                page: 1,
                count: 10,
                sorting: {
                    name: 'asc'
                }
                //filter: $scope.search
            }, {
                   // total: response.data.length,
                    getData: function (params) {

                        return AssignmentTypeService.getSearchAssignmentTypes($scope.search).then(function (response) {
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