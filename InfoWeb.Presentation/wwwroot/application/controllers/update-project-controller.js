var app = angular.module("InfoWeb");

app.controller("UpdateProjectController", ['$q', '$scope', '$state', '$stateParams', 'ProjectService', 'AuthenticationService', 'ProjectTypesService', 'ClientService', 'ngToast', function ($q, $scope, $state, $stateParams, ProjectService, AuthenticationService, ProjectTypesService, ClientService, ngToast) {
   
    var id = $stateParams.projectId;
    var userId = AuthenticationService.getCurrentUser().id;
    $scope.model = {};

    $scope.acceptButtonCaption = "Actualizar";
    $scope.title = "Actualizar Proyecto";

    var projectPromise = ProjectService.getProject(id, userId);/*.then(function (response) {       
        $scope.model = response.data;
    }, function (error) {
        });*/

    var projectTypePromise = ProjectTypesService.getProjectTypes();/*.then(function (response) {
        $scope.projectTypes = response.data;
    }, function (error) { });*/

    var clientPromise = ClientService.getCliente();/*.then(function (response) {
        $scope.clientes = response.data;
    }, function (error) { });*/

    $q.all([projectPromise, projectTypePromise, clientPromise]).then(function (response) {

        $scope.clientes = response[2].data;
        $scope.projectTypes = response[1].data;
        $scope.model = response[0].data;

    }, function (error) { });

    $scope.onButtonClicked = function () {
        ProjectService.update(userId,$scope.model).then(function (response) {
            $state.go("projectList");
            ngToast.create({
                dismissButton: true,
                content: 'El proyecto fue actualizado satisfactoriamente.'
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