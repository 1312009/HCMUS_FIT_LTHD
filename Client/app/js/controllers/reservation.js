/**
 * Created by ThaiSon on 14/12/2016.
 */

define(function (require) {
    'use strict';

    var reservation = angular.module('reservation', []);

    reservation.controller('reservation', function ($http, $scope, store, sharedData, $rootScope) {
        if(angular.isDefined(store.get('cart')) && store.get('cart') !== null)
        {
            $scope.cart = store.get('cart');

            $scope.sum = function() {
                var sumt = 0;
                var count = 0;
                angular.forEach($scope.cart, function(value, key){
                    //console.log(key + ': ' + value.Name);

                        sumt = sumt + (value.price*value.count);
                        count = count + value.count;
                });
                store.set('count',count);
                $rootScope.$emit("updateCart", {});
                return sumt;
            };

            $scope.remove = function (id) {
                for(var i = 0; i < $scope.cart.length; i ++)
                {
                    if($scope.cart[i].id == id) {
                        $scope.cart.splice(i, 1);
                    }
                }
                store.set('cart', $scope.cart);
            };
        }
    });

    return reservation;

});