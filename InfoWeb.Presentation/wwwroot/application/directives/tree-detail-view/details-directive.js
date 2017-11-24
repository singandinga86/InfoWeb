var app = angular.module("InfoWeb");

app.directive("treeDetails", function ($rootScope,$filter) {
    return {
        templateUrl: '/application/directives/tree-detail-view/details.html',
        scope: {},
        link: function (scope, element, attributes) {
            scope.$on("assignmentsDetailsChanged", function (event, assignmentDescription) {
                scope.assignments = assignmentDescription.assignments;

            });

            scope.$watch('assignments', function (newValue, oldValue) {
                var table = element.find("table");
                var body = table.find("tbody");
                elements = [];
                var rowElement = angular.element("<tr>");

                if (newValue && newValue.length >0)
                {
                   
                    angular.forEach(newValue, function (assignment) {

                        if (assignment != null && assignment != undefined)
                        {
                            var rowElement = angular.element("<tr>");
                            var columnElement = angular.element("<td>");
                            columnElement.text($filter('date')(assignment.date, 'dd/MM/yyyy'));
                            rowElement.append(columnElement);

                            columnElement = angular.element("<td>");

                            if (assignment.hours == 0)
                                columnElement.text("--");
                            else
                                columnElement.text(assignment.hours);

                            rowElement.append(columnElement);

                            columnElement = angular.element("<td>");
                            if (assignment.hourType != null)
                                columnElement.text(assignment.hourType.name);
                            else
                                columnElement.text("--");
                            rowElement.append(columnElement);

                            columnElement = angular.element("<td>");
                            columnElement.text(assignment.assignmentType.name);
                            rowElement.append(columnElement);
                        }
                        else
                        {
                            columnElement.text("No hay datos para mostrar");
                            rowElement.append(columnElement);
                        }                       
                        elements.push(rowElement);
                    });
                    body.empty();
                    body.append(elements);
                }
                else
                {
                    var columnElement = angular.element("<td colspan='4'>");
                    columnElement.text("No hay datos para mostrar");
                    rowElement.append(columnElement);
                    elements.push(rowElement);
                   
                    body.empty();
                    body.append(elements);
                }
            });
        }
    }
});