/**
 * Created by ThaiSon on 14/12/2016.
 */

define(function (require) {
    'use strict';

    var reservation = angular.module('reservation', []);

    reservation.controller('reservation', function ($http, $scope, store, sharedData) {
        if(angular.isDefined(store.get('cart')) && store.get('cart') !== null)
        {
            $scope.cart = store.get('cart');
            $scope.sum = function() {
                var sumt = 0;
                angular.forEach($scope.cart, function(value, key){
                    //console.log(key + ': ' + value.Name);

                        sumt = sumt + (value.price*value.count);

                });
                return sumt;
            };
        }
    });

    return reservation;

});