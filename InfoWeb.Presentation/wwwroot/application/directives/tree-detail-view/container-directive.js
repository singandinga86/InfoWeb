var app = angular.module("InfoWeb");

app.directive("projectTreeContainer", function () {
    return {
        restrict: 'E',
        templateUrl: '/application/directives/tree-detail-view/details.html',
        scope: {
            branch: '=',
        },
        link: function ($scope, $element, $attributes) {
           
        },
        controller: function ($scope) {
            this.setCurrentAssignmentCollection = function (assignments) {
                $scope.broadcast("assignmentChanged", { assignments: assignments });
            }
        }
    };
});