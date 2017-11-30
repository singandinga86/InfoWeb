var module = angular.module("InfoWeb");

app.controller("GroupAssignmentController", ['$scope', '$state', 'ProjectService', 'AuthenticationService', 'UserListService', 'AssignmentService', 'ngToast',
    function ($scope, $state, ProjectService, AuthenticationService, UserListService, AssignmentService, ngToast) {

        $scope.model = {}
        $scope.model.usersSelected = [];
        $scope.userDuplicate = false;
        $scope.acceptButtonCaption = "Crear";
        $scope.title = "Asignar Proyecto a Grupo de Desarrolladores";

        ProjectService.getProjectsForUser(AuthenticationService.getCurrentUser().id).then(function (response) {

            $scope.projects = response.data;
        },
            function (error) {

            });

        UserListService.getTechnicians().then(function (response) {

            $scope.users = response.data;

        }, function (error) {

            });

        //AssignmentService.getHoursType().then(function (response) {
        //    $scope.hourTypes = response.data;
        //}, function (error) { });

        $scope.onUserSelectedChanged = function () {

            var userSelected = $scope.model.user;
            var exist = $scope.model.usersSelected.filter(i => i.id === userSelected.id)[0];
            if (exist == null) {
                $scope.userDuplicate = false;
                $scope.model.usersSelected.push($scope.model.user);
            }
            else
                $scope.userDuplicate = true;

        };

        $scope.onChangedProject = function () {
            if ($scope.model.project) {
                var projectSelected = $scope.model.project;
                var userId = AuthenticationService.getCurrentUser().id;
                AssignmentService.getHoursTypeByProject(projectSelected.id, userId).then(function (response) {
                    $scope.hourTypes = response.data;
                }, function (error) { });
            }
            else
                $scope.model.hourType = null;
        };

        $scope.onRemoveUser = function (index) {
            $scope.model.usersSelected.splice(index, 1);
            $scope.model.user = null;
            $scope.userDuplicate = false;
        };

        $scope.assignProjectGroup = function () {
            
            AssignmentService.createProjectAssignmentGroup($scope.model).then(function (response) {
                $scope.model = {}
                $scope.model.usersSelected = [];
                $scope.userDuplicate = false;
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
        };

    }]);