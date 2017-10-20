﻿var app = angular.module("InfoWeb");

app.controller("RemoveHourTypeDialogController", ['$scope', '$uibModalInstance', function ($scope, $uibModalInstance) {
    $scope.msge = $scope.$resolve.target.name;


    $scope.ok = function () {
        $uibModalInstance.close(true);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
}]);