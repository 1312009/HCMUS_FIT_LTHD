/**
 * Created by ThaiSon on 01/12/2016.
 */

define(function (require) {
    'use strict';

    var home = angular.module('home', []);

    home.controller('home', function ($scope, $rootScope) {

        // $scope.ggToken = '';
        // $scope.msg = '';

        $scope.msg = "Đăng nhập thành công";
        $scope.ggToken = "access_token=" + $rootScope.accesstoken.access_token;


    });

    return home;

});