var app = angular.module("InfoWeb");

app.controller("CreateProjectController", ['$q', "$scope", '$state', "ProjectService", 'AuthenticationService', 'ProjectTypesService', 'ClientService','HourTypeService',
    function ($q, $scope, $state, ProjectService, AuthenticationService, ProjectTypesService, ClientService, HourTypeService) {
        $scope.model = {
            projectsHoursTypes:[]
        };
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear Proyecto";
 
    var userId = AuthenticationService.getCurrentUser().id;

    var projectTypePromise = ProjectTypesService.getProjectTypes();
    var clientPromise = ClientService.getCliente();
    var hourtypePromise = HourTypeService.getHourTypes();

    $scope.projectHourTypes = [];
    

    $q.all([projectTypePromise, clientPromise, hourtypePromise]).then(function (response) {
        $scope.projectTypes = response[0].data;
        $scope.clientes = response[1].data;
        $scope.hourTypes = response[2].data;
    }, function (error) { });

    $scope.onButtonClicked = function () {
        ProjectService.create(userId,$scope.model).then(function (response) {
            $state.go("projectList");
        }, function (error) {

        });
    }
}]);