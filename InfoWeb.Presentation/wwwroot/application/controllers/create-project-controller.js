var app = angular.module("InfoWeb");

app.controller("CreateProjectController", ["$scope", '$state', "ProjectService", 'AuthenticationService', 'ProjectTypesService', 'ClientService', 'ngToast', function ($scope, $state, ProjectService, AuthenticationService, ProjectTypesService, ClientService, ngToast) {
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
            ngToast.create({
                dismissButton: true,
                content: 'El proyecto fue creado satisfactoriamente.'
            });
        }, function (error) {
            ngToast.create({
                className: "danger",
                dismissButton: true,
                content: error.data.messages[0]
            });
        });
    }
}]);