var app = angular.module("InfoWeb");

app.controller("CreateProjectController", ["$scope", '$state', "ProjectService", 'AuthenticationService', 'ProjectTypesService', 'ClientService', function ($scope, $state, ProjectService, AuthenticationService, ProjectTypesService, ClientService) {
    $scope.model = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear Proyecto";
 
    var userId = AuthenticationService.getCurrentUser().id;

    ProjectTypesService.getProjectTypes().then(function (response) {
        $scope.projectTypes = response.data;
    },
        function (error) {

        });

    ClientService.getCliente().then(function (response) {
        $scope.clientes = response.data;
    },
        function (error) {
        });

    $scope.onButtonClicked = function () {
        ProjectService.create(userId,$scope.model).then(function (response) {
            $state.go("projectList");
        }, function (error) {

        });
    }
}]);