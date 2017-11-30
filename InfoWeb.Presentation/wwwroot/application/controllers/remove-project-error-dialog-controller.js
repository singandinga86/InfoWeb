
var app = angular.module("InfoWeb");

app.controller("RemoveProjetcErrorDialogController", ['$scope', '$uibModalInstance', function ($scope, $uibModalInstance) {
    $scope.msge = $scope.$resolve.targetProject.name;

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
}]);