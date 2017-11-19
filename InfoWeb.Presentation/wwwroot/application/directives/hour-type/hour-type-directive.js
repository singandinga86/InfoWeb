var app = angular.module("InfoWeb");

app.directive("hourType", function ($compile) {
    return {
        templateUrl:'/application/directives/hour-type/hour-type-directive.html',
        scope: {
            hourTypes: "=hourTypes",
            projectHourTypes: "=projectHourTypes",
            targetForm:'='
        },
        controller: function ($scope)
        {
            this.validateHourSelection = function (index) {
                var targetId = $scope.projectHourTypes[index].hourType.id;                

                for (var i = 0; i < $scope.projectHourTypes.length; i++)
                {
                    if (i != index)
                    {
                        if (targetId == $scope.projectHourTypes[i].hourType.id)
                        {
                            return false;
                        }
                    }
                }

                return true;
            };

            this.notifyHourTypeChanged = function ()
            {
                $scope.$broadcast("hourTypeValidationRequired");
            }

            this.notifyHourTypeRemoved = function (childScope, index)
            {
                $scope.projectHourTypes.splice(index, 1);   
                childScope.$destroy();
                $scope.lastDestroyedIndex = index;
                $scope.$broadcast("hourTypeRemoved");
            }

            this.getIndex = function (oldIndex)
            {
                if (oldIndex >= $scope.lastDestroyedIndex)
                {
                    return oldIndex - 1;
                }
                return oldIndex;
            }
        },
        link: function (scope, element, attributes)
        {
            var button = element.find("button");

            button.on("click", function (event) {

                scope.$apply(function () {
                   
                    scope.projectHourTypes.push({ hourType: null, hours: 1 });
                    scope.targetForm.$invalid = true;
                });   
                return false;
               
            }); 
        }
    }
});