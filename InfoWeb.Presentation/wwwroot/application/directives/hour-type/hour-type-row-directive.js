var app = angular.module("InfoWeb");

app.directive("hourTypeRow", function ($compile) {
    return {
        templateUrl:'/application/directives/hour-type/hour-type-row-directive.html',
        scope: {
            current: '=',
            all: '=',
            targetForm:'=',
            index:'@'
        },
        require: '^hourType',
        controller: function ($scope, $element)
        {
            var parentController = $element.controller('hourType');
            
            $scope.hourTypeChanged = function ()
            {
                if ($scope.current.hourType != undefined)
                {
                    parentController.notifyHourTypeChanged();
                }                
            }

            var changeFormValidity = function ()
            {
                if ($scope.hourTypeError || $scope.hourError)
                {
                    $scope.targetForm.$invalid = true;
                }
                else
                {
                    $scope.targetForm.$invalid = false;
                }

                    
            }

            var validateItems = function () {
                if (parentController.validateHourSelection($scope.index) == false) {
                    $scope.hourTypeError = true;
                    $scope.comboBox.removeClass('ng-valid');
                    $scope.comboBox.addClass('ng-invalid');
                }
                else {
                    $scope.hourTypeError = false;
                    $scope.comboBox.removeClass('ng-invalid');
                    $scope.comboBox.addClass('ng-valid');
                }

                changeFormValidity();
            };

            $scope.$on("hourTypeValidationRequired", function () {
                validateItems();
            });

            $scope.$on("hourTypeRemoved", function () {
                $scope.index = parentController.getIndex($scope.index);
                validateItems();
            });

            $scope.onHourCountChanged = function () {
                if ($scope.textBox.val() < 1)
                {
                    $scope.hourError = true;
                }
                else
                {
                    $scope.hourError = false;
                }
                changeFormValidity();
            }

            $scope.removeRow = function ($event)
            {
                var targetElement = angular.element($event.currentTarget).parent().parent();
                targetElement.remove();
                parentController.notifyHourTypeRemoved($scope, $scope.index);

               /* $scope.container.remove();
                parentController.notifyHourTypeRemoved($scope, $scope.index);*/
            }
        },
        link: function (scope, element, attributes, hourTypeController)
        {
            //scope.indexName = 'Item' + scope.index;
            scope.indexName = scope.index;
            scope.comboName = 'projectsHoursTypes[' + scope.indexName + '].hourType';
            scope.textName = 'projectsHoursTypes[' + scope.indexName + '].hours';
            scope.comboBox = element.find("select");
            scope.textBox = element.find("input.form-control");
            scope.container = element;
        }
    };
});