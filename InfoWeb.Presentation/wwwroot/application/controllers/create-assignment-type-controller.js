var app = angular.module("InfoWeb");

app.controller("CreateAssignmentTypeController", ["$scope", '$state', "AssignmentTypeService", function ($scope, $state, AssignmentTypeService) {
    $scope.model = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear tipo de asignación";

    $scope.onButtonClicked = function () {
        AssignmentTypeService.create($scope.model).then(function (response) {
            $state.go("assignmentTypeList");
        }, function (error) {

        });
    }
}]);