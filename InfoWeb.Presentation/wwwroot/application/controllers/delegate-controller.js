var module = angular.module("InfoWeb");

app.controller("ProjectDelegateController",
    ['$scope', '$state', 'ProjectService', 'AssignmentService', 'AuthenticationService', 'UserListService','ngToast',
        function ($scope, $state, ProjectService, AssignmentService, AuthenticationService, UserListService, ngToast) {

    $scope.showHoras = false;
    $scope.assignment = {};
    //$scope.assignment.hours = 35;
    $scope.project = {};

    ProjectService.getProjectsForUser(AuthenticationService.getCurrentUser().id).then(function (response) {

        $scope.projects = response.data;
    },
        function (error) {

        });

    UserListService.getUserAdmin().then(function (response) {

        $scope.users = response.data;

    }, function (error) {

        });

    AssignmentService.getAssigmentType().then(function (response) {

        $scope.assigmentTypes = response.data;
    },
        function (error) {

        });

    AssignmentService.getHoursType().then(function (response) {

        $scope.HoursTypes = response.data;       
    },
        function (error) {

        });

    $scope.changeValue = function () {
        if ($scope.selectedItem == true)
            $scope.showHoras = true;
        else
        {
            $scope.showHoras = false;
            $scope.assignment.hourType = null;
            $scope.assignment.hours = null;
        }
           
    };

    $scope.send = function () {
        console.log($scope.assignment);
        $scope.assignment.project =  $scope.assignment.project;
        AssignmentService.createAssigment($scope.assignment).then(function (response) {           
            $scope.assignment = {};
            $scope.showHoras = false;
            $state.go("projectList");
            ngToast.create({
                dismissButton: true,
                content: 'El proyecto fue delegado satisfactoriamente.'
            });
        }, function (error) {
            //console.log(error);
            if (error.data.messages != null && error.data.messages != undefined)
            {
                ngToast.create({
                    className: "danger",
                    dismissButton: true,
                    content: error.data.messages[0]
                });
            } else
            {
                ngToast.create({
                    className: "danger",
                    dismissButton: true,
                    content: "Ocurrió un error."
                });
            }
          
        });

    };

    $scope.onChangeProjectDelegate = function () {
        var project = $scope.assignment.project;
        //console.log(project);
        AssignmentService.getHoursTypeByProject(project.id).then(function (response) {

            $scope.HoursTypes = response.data;
        },
            function (error) {

            });
    };

    //$scope.changeValue = function () {
    //    $scope.assignment.hours = 5;
    //};



}]);