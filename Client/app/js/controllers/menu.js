/**
 * Created by ThaiSon on 09/12/2016.
 */

define(function (require) {
    'use strict';

    var menu = angular.module('menu', []);

    menu.controller('menu', function ($http, $scope, store, sharedData, $rootScope) {

        $scope.listFood = sharedData.listFood;

        $scope.cart = [];
        $scope.count = 0;
        if(angular.isDefined(store.get('cart')) && store.get('cart') !== null)
        {
            $scope.cart = store.get('cart');
            $scope.count = store.get('count');
        }

        $scope.item = {
            name: null,
            price: null,
            imgfood: null,
            count: 1,
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
        console.log($scope.cart);
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

    return menu;

});