/**
 * Created by ThaiSon on 24/12/2016.
 */

define(function (require) {
    'use strict';

    var payment = angular.module('payment', []);

    payment.controller('payment', function ($http, store, $scope, $state) {
        $scope.userName = store.get('jwt').dbUser.name;
        console.log($scope.userName);
    });

    return payment;

});