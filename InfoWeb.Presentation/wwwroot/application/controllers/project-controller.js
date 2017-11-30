var module = angular.module("InfoWeb");

app.controller("ProjectController", ['$scope', '$uibModal', 'ProjectService', 'AuthenticationService', '$state', '$filter', 'NgTableParams', 'ngToast', function ($scope, $uibModal, ProjectService, AuthenticationService, $state, $filter, NgTableParams, ngToast) {

    $scope.acceptButtonCaption = "Nuevo";
    $scope.currentUser = AuthenticationService.getCurrentUser();



    var fillTable = function () {
        //ProjectService.getProjectsForUser(AuthenticationService.getCurrentUser().id).then(function (response) {

            $scope.model = {};
            $scope.search = "";
            var orderedData = [];


            
            $scope.model.tableParams = new NgTableParams({
                page: 1,
                count: 10,
                //filter: $scope.search,
                sorting: {
                    name: 'asc'
                }
            }, {
                   // total: response.data.length,
                    getData: function (params) {                     

                        return ProjectService.searchProjects(AuthenticationService.getCurrentUser().id, $scope.search).then(function (response) {
                            orderedData = $filter('orderBy')(response.data, params.orderBy());
                            
                            params.total(orderedData.length);                       
                            return orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        }, function (error) {

                        });
                    }
                }
            );

            $scope.$watch("search", function () {
               
                $scope.model.tableParams.reload();
               
            });
            //var searchData = function (data) {
            //    console.log('entro');
            //    console.log($scope.search);
            //    if ($scope.search != '')
            //    {
            //        console.log("sin filtrar: ");
            //        console.log(data);

            //        var filtrado = $filter('filter')(data, $scope.search);
            //        console.log("filtrado: ");
            //        console.log(data);

            //        return filtrado;
            //    }
            //    console.log(response.data);
            //    return response.data;
            //}


        //},
        //    function (error) {

        //    });
    }

    fillTable();


    $scope.viewDetails = function (id) {
        $state.go('projectDetails', { projectId: id });
    };

        $scope.onRemoveClicked = function (project) {

            ProjectService.canBeRemoved(project.id, $scope.currentUser.id).then(function (response) {
                var dialog = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'application/views/project/remove-modal.html',
                    controller: "RemoveProjetcDialogController",
                    resolve: {
                        targetProject: function () { return project; }
                    }
                });

                dialog.result.then(function (result) {
                    if (result == true) {
                        ProjectService.removeProject($scope.currentUser.id, project.id).then(function (response) {
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
                }, function (error) {

                });
            }, function (error) {
                var dialog = $uibModal.open({
                    animation: true,
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: 'application/views/project/remove-error.html',
                    controller: "RemoveProjetcErrorDialogController",
                    resolve: {
                        targetProject: function () { return project; }
                    }
                }).result.catch(function (res) {
                    if (!(res === 'cancel' || res === 'escape key press')) {
                        throw res;
                    }
                });
                });          


    }

    $scope.onCreateClicked = function () {
        $state.go("createProject");
    }
    $scope.onEditClicked = function (id) {
        $state.go('updateProject', { projectId: id })
    }

    $scope.onViewHoursClicked = function (project) {


        $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'application/views/project/details-modal.html',
            controller: "ProjectDetailModalController",
            windowClass: 'clsPopup',
            resolve: {
                targetProject: function () {
                    return project.id;
                }
            }
        });


    }

    //$scope.$watch("search", function () {
    //    console.log("cambio");
    //    fillTable();
    //});






}]);