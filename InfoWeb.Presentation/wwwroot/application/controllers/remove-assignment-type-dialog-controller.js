var app = angular.module("InfoWeb");

app.controller("RemoveAssignmentTypeDialogController", ['$scope', '$uibModalInstance', function ($scope, $uibModalInstance) {
    $scope.msge = $scope.$resolve.targetRole.name;


    $scope.ok = function () {
        $uibModalInstance.close(true);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
}]);