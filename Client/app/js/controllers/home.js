/**
 * Created by ThaiSon on 01/12/2016.
 */

define(function (require) {
    'use strict';

    var home = angular.module('home', []);

    home.controller('home', function ($state, $scope, store, sharedData, $timeout, $rootScope) {

        $timeout(function () {
            $scope.listFood = sharedData.listFood;
        }, 200);

        var meals = ["","",""];
        $scope.gotoMenu = function (id) {
            meals[id] = "selected";
            sharedData.meals = meals;
            $state.go("menu");
        };

        $scope.item = {
            name: null,
            price: null,
            imgfood: null,
            count: 1,
        };
        $scope.cart = [];
        $scope.count = 0;
        if(angular.isDefined(store.get('cart')) && store.get('cart') !== null)
        {
            $scope.cart = store.get('cart');
            $scope.count = store.get('count');
        }

        $scope.success = function (id) {
            $scope.item.name = $scope.listFood[id].name;
            $scope.item.price = $scope.listFood[id].price;
            $scope.item.imgfood = $scope.listFood[id].imgfood;

            if($scope.cart.length > 0)            {
                var exist = false;
                for(var i = 0; i < $scope.cart.length; i++)
                {
                    if($scope.cart[i].name == $scope.item.name) {
                        $scope.cart[i].count++;
                        exist = true;
                        break;
                    }

                }
                if(!exist)
                {
                    $scope.cart.push({
                        count: $scope.item.count,
                        name: $scope.item.name,
                        price: $scope.item.price,
                        imgfood: $scope.item.imgfood
                    });
                }
            }
            else
            {
                $scope.cart.push({
                    count: $scope.item.count,
                    name: $scope.item.name,
                    price: $scope.item.price,
                    imgfood: $scope.item.imgfood
                });
            }

            $scope.count++;

            store.set('cart', $scope.cart);
            store.set('count', $scope.count);
            console.log($scope.cart.indexOf({
                count: $scope.item.count,
                name: $scope.item.name,
                price: $scope.item.price,
                imgfood: $scope.item.imgfood
            }));
            console.log($scope.item);
            console.log($scope.cart);
            $rootScope.$emit("updateCart", {});
        };
    });

    return home;

});