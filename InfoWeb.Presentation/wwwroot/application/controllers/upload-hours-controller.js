var module = angular.module("InfoWeb");

app.controller("UploadHoursController", ['$scope', '$state', 'ProjectService',
                'AuthenticationService', 'UserListService', 'AssignmentService',
                'ngToast', 'WorkedHoursService',
                function ($scope, $state, ProjectService, AuthenticationService, UserListService, AssignmentService, ngToast, WorkedHoursService) {

                    $scope.model = {}    
                    $scope.acceptButtonCaption = "Subir";
                    $scope.title = "Subir Horas";
                    $scope.elapsedHours = 0;

                    ProjectService.getProjectsForUser(AuthenticationService.getCurrentUser().id).then(function (response) {

                        $scope.projects = response.data;
                    },
                    function (error) {

                    });
                    $scope.onChangedProject = function () {
                        if ($scope.model.project) {
                            var projectSelected = $scope.model.project;
                            var userId = AuthenticationService.getCurrentUser().id;
                            AssignmentService.getHoursTypeByAssignment(projectSelected.id, userId).then(function (response) {
                                $scope.hourTypes = response.data;
                            }, function (error) { });

                           // console.log(projectSelected);
                
            ;
                        }
                        else
                            $scope.model.hourType = null;
                    };

                    $scope.today = function () {
                        alert('asdasd');
                    }

                    $scope.uploadHours = function () {
                        var userId = AuthenticationService.getCurrentUser().id;
                        WorkedHoursService.uploadHours(userId, $scope.model).then(function (response) {
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

                    $scope.onDatesChanged = function ()
                    {
                        
                        if ($scope.model.dateStart && $scope.model.dateEnd)
                        {
                            var start = new Date($scope.model.dateStart);
                            var end = new Date($scope.model.dateEnd);

                            var hours = Math.abs(end - start) / 36e5;

                            $scope.elapsedHours = hours;
                        }
                    }

        

    }]);