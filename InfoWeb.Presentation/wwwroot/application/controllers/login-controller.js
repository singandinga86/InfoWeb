var module = angular.module("InfoWeb");

app.controller("LoginController", ['$scope', '$rootScope', 'AuthenticationService', 'NotificationService', '$uibModal', function ($scope, $rootScope, AuthenticationService, NotificationService, $uibModal) {

    $scope.userName = "";
    $scope.password = "";
    $scope.invalidCredentials = false;
    angular.element('#userName').focus();
    var modal = null;

    $scope.onLoginClicked = function () {
        modal = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'application/views/login/loading-modal.html',
            controller: "LoginController",
            windowClass: 'clsPopupLoad',
        });
        AuthenticationService.login($scope.userName, $scope.password).then(function (response) {
            $rootScope.$broadcast("userAuthenticated", response.data);
            $scope.invalidCredentials = false;
            modal.close();
        }, function (error) {
            $scope.invalidCredentials = true;
            modal.close();
        });
    }; 

    $scope.sendForm = function ($event) {   
       
        if ($event.keyCode === 13) {            
            $event.preventDefault();
            modal = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'application/views/login/loading-modal.html',
                controller: "LoginController",
                windowClass: 'clsPopupLoad',
            });
            AuthenticationService.login($scope.userName, $scope.password).then(function (response) {
                $rootScope.$broadcast("userAuthenticated", response.data);
                $scope.invalidCredentials = false;
                modal.close();
            }, function (error) {
                $scope.invalidCredentials = true;
                modal.close();
            });
        }

    }
}
]);