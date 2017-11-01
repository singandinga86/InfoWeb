var app = angular.module("InfoWeb");

app.controller("UpdateProjectController", ['$scope', '$state', '$stateParams', 'ProjectService', 'AuthenticationService', function ($scope, $state, $stateParams, ProjectService, AuthenticationService) {
   
    var id = $stateParams.projectId;
    var userId = AuthenticationService.getCurrentUser().id;
    $scope.model = {};

    $scope.acceptButtonCaption = "Actualizar";
    $scope.title = "Actualizar Proyecto";

    ProjectService.getProject(id, userId).then(function (response) {       
        $scope.model = response.data;
    }, function (error) { });

    $scope.onButtonClicked = function () {
        ProjectService.update(userId,$scope.model).then(function (response) {
            $state.go("projectList");
        }, function (error) { });
    }
}]);