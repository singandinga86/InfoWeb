var module = angular.module("InfoWeb");

app.controller("LoginController", ['$scope', '$rootScope', 'AuthenticationService', 'NotificationService', function ($scope, $rootScope, AuthenticationService, NotificationService) {

    $scope.userName = "";
    $scope.password = "";
    $scope.invalidCredentials = false;
    angular.element('#userName').focus();

    $scope.onLoginClicked = function () {
        AuthenticationService.login($scope.userName, $scope.password).then(function (response) {
            $rootScope.$broadcast("userAuthenticated", response.data);
            $scope.invalidCredentials = false;
        }, function (error) {
            $scope.invalidCredentials = true;
        });
    }; 

    $scope.sendForm = function ($event) {       
        if ($event.keyCode === 13) {            
            $event.preventDefault();
            AuthenticationService.login($scope.userName, $scope.password).then(function (response) {
                $rootScope.$broadcast("userAuthenticated", response.data);
                $scope.invalidCredentials = false;
            }, function (error) {
                $scope.invalidCredentials = true;
            });
        }

    }
}
]);