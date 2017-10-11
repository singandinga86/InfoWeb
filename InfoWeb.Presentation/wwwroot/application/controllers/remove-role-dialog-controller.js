var app = angular.module("InfoWeb");

app.controller("RemoveRoleDialogController", ['$scope','$uibModalInstance',function ($scope,$uibModalInstance) {
    
    $scope.ok = function () {
        $uibModalInstance.close(true);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
}]);