var app = angular.module("InfoWeb");

app.controller("CreateAssignmentTypeController", ["$scope", '$state', "AssignmentTypeService",'ngToast',
    function ($scope, $state, AssignmentTypeService, ngToast) {
    $scope.model = {};
    $scope.acceptButtonCaption = "Crear";
    $scope.title = "Crear Tipo de Asignación";

    $scope.onButtonClicked = function () {
        AssignmentTypeService.create($scope.model).then(function (response) {
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