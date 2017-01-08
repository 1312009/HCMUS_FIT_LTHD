/**
 * Created by ThaiSon on 01/12/2016.
 */

define(function (require) {
    'use strict';

    var home = angular.module('home', []);

    home.controller('home', function ($http, $state, $scope, store, sharedData, $timeout, $rootScope, Notification) {

        $scope.listFood = sharedData.listFood;
        $scope.listRef = "";

        $rootScope.$on("getFoods", function(){
            $scope.listFood = sharedData.listFood;
            $scope.listFood.sort(function (a, b) {
                return a.number - b.number;
            });
            console.log($scope.listFood);
        });

        $http({
            url: sharedData.host + '/api/foods/RutTrichThongTin',
            method: 'GET',
        }).then(function(response) {
            $scope.listRef = response.data;
            console.log(response.data);
        }, function(error) {
            console.log(error);
        });

        var meals = ["","",""];
        $scope.gotoMenu = function (id) {
            meals[id] = "selected";
            sharedData.meals = meals;
            $rootScope.$emit("setMenu", 1);
            $state.go("menu");
        };

        $scope.item = {
            name: null,
            price: null,
            imgfood: null,
            id: null,
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

    return home;

});