/**
 * Created by ThaiSon on 09/12/2016.
 */

define(function (require) {
    'use strict';

    var menu = angular.module('menu', []);

    menu.controller('menu', function ($http, $scope, store, sharedData) {

        $scope.listFood = sharedData.listFood;
        $scope.toggle = false;
        $scope.idtype = 1;
        $scope.breakFirst = "selected";
        $scope.lunch = "";
        $scope.dinner = "";

        $scope.breakFirstClick = function () {
            $scope.idtype = 1;
            $scope.breakFirst = "selected";
            $scope.lunch = "";
            $scope.dinner = "";
        };

        $scope.lunchClick = function () {
            $scope.idtype = 2;
            $scope.breakFirst = "";
            $scope.lunch = "selected";
            $scope.dinner = "";
        };

        $scope.dinnerClick = function () {
            $scope.idtype = 3;
            $scope.breakFirst = "";
            $scope.lunch = "";
            $scope.dinner = "selected";
        };
    });

    return menu;

});