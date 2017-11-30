var app = angular.module("InfoWeb");

app.controller("UpdateAssignmentTypeController", ['$scope', '$state', '$stateParams', 'AssignmentTypeService','ngToast',
    function ($scope, $state, $stateParams, AssignmentTypeService, ngToast) {

    var id = $stateParams.id;
    $scope.model = {};

    $scope.acceptButtonCaption = "Actualizar";
    $scope.title = "Actualizar tipo de asignación";

    AssignmentTypeService.getAssignmentType(id).then(function (response) {
        $scope.model = response.data;
    }, function (error) { });

    $scope.onButtonClicked = function () {
        AssignmentTypeService.update($scope.model).then(function (response) {
            $state.go("assignmentTypeList");
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
}]);