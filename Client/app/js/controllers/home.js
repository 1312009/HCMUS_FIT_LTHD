/**
 * Created by ThaiSon on 01/12/2016.
 */

define(function (require) {
    'use strict';

    var home = angular.module('home', []);

    home.controller('home', function ($http, $scope, store) {

        $scope.msg = "Đăng nhập thành công";
        $scope.accessToken = "access_token=" + store.get('accessToken').access_token;

        $scope.jwt = store.get('jwt');
        callApi('Secured', 'http://localhost:59219/api/foods');


        function callApi(type, url) {
            // $scope.response = null;
            $scope.api = type;
            $http({
                url: url,
                method: 'GET',
                headers: {
                    Authorization: $scope.jwt
                }
            }).then(function(quote) {
                $scope.listFood = quote.data;
                console.log($scope.listFood);
                console.log($scope.listFood[0].name);
            }, function(error) {
                console.log(error);
            });
        }
    });

    return home;

});