var module = angular.module("InfoWeb");

app.controller("ProjectController",
    ['$scope','$uibModal' ,'ProjectService', 'AuthenticationService', '$state', '$filter', 'NgTableParams',
        function ($scope, $uibModal,ProjectService, AuthenticationService, $state, $filter, NgTableParams) {


    ProjectService.getProjectsForUser(AuthenticationService.getCurrentUser().id).then(function (response) {

        $scope.model = {};
        $scope.search = { term: '' };
        var orderedData = [];

        //$scope.model.tableParams = new NgTableParams({}, { dataset: response.data });
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
        $scope.viewDetails = function (id) {
            $state.go('projectDetails', { projectId: id });
        };

    },
        function (error) {

                });

    $scope.onRemoveClicked = function (id) {
        var dialog = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'application/views/project/remove-modal.html',
            controller: "RemoveProjetcDialogController",
            resolve: {
                targetRole: function () { return id; }
            }
        });

        dialog.result.then(function (result) {
            if (result == true) {
                ProjectService.removeProject(id).then(function (response) {
                    fillTable();
                }, function (error) { });

            }
        }, function () {

        });


    }
}]);