var app = angular.module("InfoWeb");

app.controller("ProjectDetailModalController", ['$scope', '$uibModalInstance', 'ProjectService', 'AuthenticationService', function ($scope, $uibModalInstance, ProjectService, AuthenticationService) {

    var userId = AuthenticationService.getCurrentUser().id;
    var projectId = null;
    if ($scope.$resolve.targetProject != null || $scope.$resolve.targetProject != undefined)
        projectId = $scope.$resolve.targetProject;

    ProjectService.getProjectDetails(userId, projectId).then(function (response) {
        var data = response.data;

        
        $scope.HoursDetails = data.hoursDetails;
        $scope.project = data.project;
    },
        function (error) { });

    $scope.cancel = function () {
        $uibModalInstance.close();
    };
}]);