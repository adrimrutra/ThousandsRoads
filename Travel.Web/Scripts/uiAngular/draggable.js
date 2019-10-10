/*
 jQuery UI Draggable plugin wrapper
*/

angular.module('ui.draggable', [])
    .directive('uiSelectable', function ($parse) {
    return {
        link: function (scope, element, attrs, ctrl) {

            scope.$on('clearselection', function (event, document) {
                element.find('.ui-selected').removeClass('ui-selected')
            });

            element.selectable({
                stop: function (evt, ui) {
                    var collection = scope.$eval(attrs.docArray)
                    var selected = element.find('div.parent.ui-selected').map(function () {
                        var idx = $(this).index();
                        return { document: collection[idx] }
                    }).get();

                    scope.selectedItems = selected;
                    scope.$apply()
                }
            });
        }
    }
})
    .directive('uiDraggable', function () {
        return {
            require: 'ngModel',
            link: function ($scope, $element, $attrs, ngModel) {

                function combineCallbacks(first, second) {
                    if (second && (typeof second === "function")) {
                        return function (event, ui) {
                            first(event, ui);
                            second(event, ui);
                        };
                    }
                    return first;
                }

                var options = {
                    connectToSortable: $attrs.uiDraggable,
                    cursor: "move",
                    helper: 'clone',
                    cancel: ".disabled"
                };

                var callbacks = {
                    start: function (event, ui) {
                        $(".ui-draggable-dragging").css('min-width', $element.width());
                        $scope.$viewValue = ngModel.$viewValue;
                    }
                };

                angular.forEach(callbacks, function (value, key) {
                    options[key] = combineCallbacks(value, options[key]);
                });

                $element.draggable(options);
                $element.disableSelection();
            }
        }
    });