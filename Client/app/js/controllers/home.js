/**
 * Created by ThaiSon on 01/12/2016.
 */

define(function (require) {
    'use strict';

    var home = angular.module('home', []);

    home.controller('home', function ($http, $scope, store, sharedData, $timeout) {

        // $scope.msg = "Đăng nhập thành công";
        // $scope.jwt = store.get('jwt');
        // $scope.accessToken = "access_token=" + store.get('accessToken').access_token;

        $timeout(function () {
            $scope.listFood = sharedData.listFood;
        }, 200);


    });

    return home;

});