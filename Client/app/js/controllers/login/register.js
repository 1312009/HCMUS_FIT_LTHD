/**
 * Created by ThaiSon on 01/01/2017.
 */

define(function (require) {
    'use strict';

    var register = angular.module('register', []);

    register.controller('register', function ($http, store, $scope, $state, $rootScope) {
        console.log("Register loaded!");
    });

    return register;

});