var app = angular.module("InfoWeb");

app.controller("UpdateAssignmentTypeController", ['$scope', '$state', '$stateParams', 'AssignmentTypeService', function ($scope, $state, $stateParams, AssignmentTypeService) {

    var id = $stateParams.id;
    $scope.model = {};

    $scope.acceptButtonCaption = "Actualizar";
    $scope.title = "Actualizar tipo de asignación";

    AssignmentTypeService.getAssignmentType(id).then(function (response) {
        $scope.model = response.data;
    }, function (error) { });

    $scope.onButtonClicked = function () {
        AssignmentTypeService.update($scope.model).then(function (response) {
            $state.go("hourTypeList");
        }, function (error) { });
    }
}]);