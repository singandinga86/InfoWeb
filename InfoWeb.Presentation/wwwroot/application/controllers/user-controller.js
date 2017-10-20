﻿var module = angular.module("InfoWeb");

app.controller("UserListController", ['$scope', 'UserListService', '$filter', 'NgTableParams', function ($scope, UserListService, $filter, NgTableParams) {

    UserListService.getUser().then(function (response) {

        $scope.model = {};
        $scope.search = { term: '' };
        var orderedData = [];

        //$scope.model.tableParams = new NgTableParams({}, { dataset: response.data });
        $scope.model.tableParams = new NgTableParams({
            page: 1,
            count: 6,

            filter: $scope.search
        },
            {
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

    },
        function (error) {

            fillTable();

            $scope.onCreateClicked = function () {
                $state.go("createUser");
            }

            $scope.onEditClicked = function (id) {
                $state.go('updateUser', { userId: id })
            }

            $scope.onRemoveClicked = function (user) {
                var dialog = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'application/views/user/remove-modal.html',
                    controller: "RemoveUserDialogController",
                    resolve: {
                        targetRole: function () { return user; }
                    }
                });

                dialog.result.then(function (result) {
                    if (result == true) {
                        UserListService.remove(user.id).then(function (response) {
                            fillTable();
                        }, function (error) { });

                    }
                }, function () {

                });

            }
        });
}]);