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

        callApi('Secured', 'http://localhost:59219/api/foods/GetAllFoods');


        function callApi(type, url) {
            // $scope.response = null;
            $scope.api = type;
            $http({
                url: url,
                method: 'GET',
            }).then(function(response) {
                sharedData.listFood = response.data;
                $scope.listFood = response.data;
                console.log($scope.listFood);
            }, function(error) {
                console.log(error);
            });
        }


    });

    return home;

});