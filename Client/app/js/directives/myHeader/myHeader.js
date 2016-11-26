/**
 * Created by ThaiSon on 26/11/2016.
 */

define(function (require) {
    'use strict';

    var header = angular.module('header', []);

    header.directive('myHeader', function () {
        return {
            restrict: 'E',
            link: function(scope, element, attribute)
            {
                $(document).ready(function(){
                    $(".bt-menu-trigger").toggle(
                        function(){
                            $('.bt-menu').addClass('bt-menu-open');
                        },
                        function(){
                            $('.bt-menu').removeClass('bt-menu-open');
                        }
                    );
                });
            },

            templateUrl: 'js/directives/myHeader/header.html'
        };
    });

    return header;

});