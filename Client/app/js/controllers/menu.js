/**
 * Created by ThaiSon on 09/12/2016.
 */

define(function (require) {
    'use strict';

    var menu = angular.module('menu', []);

    menu.controller('menu', function ($http, $scope, store, sharedData, $rootScope) {

        $scope.listFood = sharedData.listFood;

        $scope.cart = [];
        if(angular.isDefined(store.get('cart')) && store.get('cart') !== null)
        {
            $scope.cart = store.get('cart');
        }

        $scope.item = {
            name: null,
            price: null
        };
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

        $scope.success = function (id) {
            $scope.item.name = $scope.listFood[id].name;
            $scope.item.price = $scope.listFood[id].price;
            $scope.cart.push($scope.item);
            store.set('cart', $scope.cart);
            $rootScope.$emit("updateCart", {});
        };
    });

    return menu;

});