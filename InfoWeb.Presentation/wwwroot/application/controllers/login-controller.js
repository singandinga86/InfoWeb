var module = angular.module("InfoWeb");

app.controller("LoginController", ['$scope','$rootScope','AuthenticationService',  function ($scope, $rootScope, AuthenticationService) {

    $scope.userName = "";
    $scope.password = "";
    $scope.invalidCredentials = false;

    $scope.onLoginClicked = function () {
        AuthenticationService.login($scope.userName, $scope.password).then(function (response) {
            $rootScope.$broadcast("userAuthenticated", response.data);
            $scope.invalidCredentials = false;
        }, function (error) {
            $scope.invalidCredentials = true;
        });
    };    
}
]);