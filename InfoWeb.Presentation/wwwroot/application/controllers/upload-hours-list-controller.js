var app = angular.module("InfoWeb");

app.controller("UploadHoursListController", ['$scope', '$state', '$uibModal', 'WorkedHoursService', 'AuthenticationService','UserListService', '$filter', 'NgTableParams', 'ngToast',
    function ($scope, $state, $uibModal, WorkedHoursService, AuthenticationService, UserListService, $filter, NgTableParams, ngToast) {

        $scope.model = {};
        $scope.search = "";        
        var orderedData = [];
        var currentUser = AuthenticationService.getCurrentUser();

        UserListService.getTechnicians().then(function (response) {            
            $scope.technicians = response.data;            
        }, function (error) { });
        
        var fillTable = function () {

            // ProjectTypesService.getProjectTypes().then(function (response) {
            $scope.model.tableParams = new NgTableParams({
                page: 1,
                count: 10,
                sorting: {
                    name: 'asc'
                }
                //filter: $scope.search
            }, {
                    //total: response.data.length,
                    getData: function (params) {

                        return WorkedHoursService.getWorkedHours(currentUser,$scope.search).then(function (response) {
                            //console.log(response.data);
                            orderedData = $filter('orderBy')(response.data, params.orderBy());

                            params.total(orderedData.length);
                            return orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        }, function (error) {

                        });
                    }
                }
            );
            //}, function (error) { });
            /*$scope.$watch("search", function () {

                $scope.model.tableParams.reload();

            });*/
        }


        $scope.onSearchChanged = function ()
        {
            if ($scope.model.tableParams)
            {
                $scope.model.tableParams.reload();
            }
        }

        fillTable();

        $scope.onTechnicianChanged = function () {
            currentUser = $scope.technician;
            fillTable();
        }

        $scope.onEditClicked = function (id) {
            $state.go('updateProjectTypes', { id: id })
        }

        $scope.onRemoveClicked = function (target) {
            var dialog = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'application/views/project-types/remove-modal.html',
                controller: "RemoveProjectTypesDialogController",
                resolve: {
                    target: function () { return target; }
                }
            });

            dialog.result.then(function (result) {
                if (result == true) {
                    ProjectTypesService.remove(target.id).then(function (response) {
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