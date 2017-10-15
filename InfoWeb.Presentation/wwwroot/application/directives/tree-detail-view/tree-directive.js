

angular
    .module('InfoWeb')
    .directive("tree", function ($compile, $rootScope) {
        return {
            restrict: "E",
            scope: {
                branch: '='
            },
            templateUrl: 'application/directives/tree-detail-view/tree.html',
            compile: function (tElement, tAttr) {
                var contents = tElement.contents().remove();
                var compiledContents;
                
                return function (scope, iElement, iAttr, projectTreeContainer) {
                    
                    if (!compiledContents) {
                        compiledContents = $compile(contents);
                    }
                    compiledContents(scope, function (clone, scope) {
                        iElement.append(clone);
                    });

                };

                
            },
            controller: function ($scope, $rootScope) {               

                $scope.toggle = function (root, setting, depth) {
                    if (!depth) {
                        depth = 0
                    }
                    if (setting === null || setting === undefined) {
                        setting = !root.isSelected;
                    }
                    root.isSelected = setting;

                    if (root.children)
                    {
                        root.children.forEach(function (branch) {
                            $scope.toggle(branch, setting, depth + 1);
                        });
                    }
                    
                    if (depth === 0) {
                        $scope.checkParent(root.parent);
                    }
                }

                $scope.expand = function (root, setting) {
                    if (!setting) {
                        setting = !root.isExpanded;
                    }
                    root.isExpanded = setting;

                    if (root.children)
                    {
                        root.children.forEach(function (branch) {
                            $scope.expand(branch, setting);
                        });
                    }                    
                }

                $scope.checkParent = function (root) {
                    var selected = false;
                    root.children.forEach(function (branch) {
                        if (branch.isSelected) {
                            selected = true;
                        }
                    });
                    root.isSelected = selected;
                    $scope.checkParent(root.parent);
                }

                $scope.setCurrentAssigments = function (assignments)
                {
                    $rootScope.$broadcast("assignmentsDetailsChanged", { assignments });
                }


            }
        };
    });