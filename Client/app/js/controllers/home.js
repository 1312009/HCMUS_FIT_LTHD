/**
 * Created by ThaiSon on 01/12/2016.
 */

define(function (require) {
    'use strict';

    var home = angular.module('home', []);

    home.controller('home', function ($state, $scope, store, sharedData, $timeout) {

        $timeout(function () {
            $scope.listFood = sharedData.listFood;
        }, 200);

        var meals = ["","",""];
        $scope.gotoMenu = function (id) {
            meals[id] = "selected";
            sharedData.meals = meals;
            $state.go("menu");
        };

    });

    return home;

});