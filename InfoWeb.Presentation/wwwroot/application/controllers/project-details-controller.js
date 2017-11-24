

angular
    .module("InfoWeb")
    .controller("ProjectDetailsController", ProjectDetailsController);

ProjectDetailsController.$inject = ["$scope", "$stateParams", "ProjectService", "AuthenticationService"];

function ProjectDetailsController($scope, $stateParams, ProjectService, AuthenticationService) {
    var vm = this;
    vm.expandAll = expandAll;
   
    var userId = AuthenticationService.getCurrentUser().id;
    var projectId = null;
    if ($scope.$resolve.targetProject == null || $scope.$resolve.targetProject == undefined)
        projectId = $stateParams.projectId;
    else
        projectId = $scope.$resolve.targetProject;
   

    ProjectService.getProjectDetails(userId, projectId).then(function (response) {
        var data = response.data;

        var addExpansionDetails = function (assignmentDescription, parent) {
            assignmentDescription.isExpanded = false;
            assignmentDescription.isSelected = false;
            assignmentDescription.parent = parent;

            if (assignmentDescription.innerAssignments.length > 0) {
                assignmentDescription.innerAssignments.forEach(function (item) {
                    addExpansionDetails(item, assignmentDescription);
                });
            }
        }

        var details = data.details;
       
        var root = {
            user: data.user,
            project: data.project,
            innerAssignments: null,
            assignments: data.assignments,           
            isExpanded: false,
            isSelected: false,
        }

        details.forEach(function (userAssignmentsDetails) {
            addExpansionDetails(userAssignmentsDetails, root);
        });

        root.innerAssignments = details;

        vm.innerAssignments = root;
        $scope.innerAssignments = root;
        $scope.HoursDetails = data.hoursDetails;
        $scope.project = data.project;
    },
        function (error) { });

    function expandAll(root, setting) {
        if (!setting) {
            setting = !root.isExpanded;
        }
        root.isExpanded = setting;
        root.children.forEach(function (branch) {
            expandAll(branch, setting);
        });
    }

}