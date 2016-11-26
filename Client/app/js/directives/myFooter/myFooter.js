/**
 * Created by ThaiSon on 26/11/2016.
 */


define(function (require) {
    'use strict';

    var footer = angular.module('footer', []);

    footer.directive('myFooter', function () {
        return {
            restrict: 'E',
            templateUrl: 'js/directives/myFooter/footer.html'
        };
    });

    return footer;

});