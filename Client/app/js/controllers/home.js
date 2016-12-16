/**
 * Created by ThaiSon on 01/12/2016.
 */

define(function (require) {
    'use strict';

    var home = angular.module('home', []);

    home.controller('home', function ($http, $scope, store, sharedData) {

        // $scope.msg = "Đăng nhập thành công";
        // $scope.jwt = store.get('jwt');
        // $scope.accessToken = "access_token=" + store.get('accessToken').access_token;




    });

    return home;

});