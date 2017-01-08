/**
 * Created by ThaiSon on 09/12/2016.
 */

define(function (require) {
    'use strict';

    var menu = angular.module('menu', []);

    menu.controller('menu', function ($http, $scope, store, sharedData, $rootScope, $timeout, Notification) {

        $scope.listFood = sharedData.listFood;

        $rootScope.$on("getFoods", function(){
            $scope.listFood = sharedData.listFood;
        });

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
            id: null,
            count: 1,
        };
        $scope.toggle = false;
        $scope.idtype = 1;
        $scope.meals = sharedData.meals;

        $scope.changeMeals = function (id) {
            $scope.idtype = id;
            id = id - 1;
            for(var i = 0; i < 3; i++)
            {
                if(i === id)
                    $scope.meals[i] = "selected";
                else
                    $scope.meals[i] = "";
            }
        };

        $scope.success = function (id) {
            $scope.item.name = $scope.listFood[id].name;
            $scope.item.price = $scope.listFood[id].price;
            $scope.item.imgfood = $scope.listFood[id].imgfood;
            $scope.item.id = $scope.listFood[id].id;

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
                        id: $scope.item.id,
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
                    id: $scope.item.id,
                    imgfood: $scope.item.imgfood
                });
            }

            $scope.count++;

            store.set('cart', $scope.cart);
            store.set('count', $scope.count);

            $rootScope.$emit("updateCart", {});

            Notification.info({message: 'Đã thêm vào giỏ hàng!', delay: 1500});
        };
    });

    return menu;

});