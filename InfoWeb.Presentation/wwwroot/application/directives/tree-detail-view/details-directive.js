var app = angular.module("InfoWeb");

app.directive("treeDetails", function ($rootScope) {
    return {
        templateUrl: '/application/directives/tree-detail-view/details.html',
        scope: {},
        link: function (scope, element, attributes)
        {
            scope.$on("assignmentsDetailsChanged", function (event, assignmentDescription) {
                scope.assignments = assignmentDescription.assignments;

            });            

            scope.$watch('assignments', function (newValue, oldValue) {
                if (scope.assignments)
                {
                    var table = element.find("<table>");
                    var body = table.find("<tbody>");

                    var newBodyElement = angular.element("<tbody>");

                    angular.forEach(scope.assignments, function (assignment) {

                        var rowElement = angular.element("<tr>");

                        var columnElement = angular.element("<td>");
                        columnElement.text(assignment.date);
                        rowElement.append(columnElement);

                        columnElement = angular.element("<td>");
                        columnElement.text(assignment.hours);
                        rowElement.append(columnElement);

                        columnElement = angular.element("<td>");
                        columnElement.text(assignment.hourTypeId);
                        rowElement.append(columnElement);

                        newBodyElement.append(rowElement);
                       
                    });
                    table.remove("tbody");
                    table.append(newBodyElement);
                }

                

            });
        }
    }
});