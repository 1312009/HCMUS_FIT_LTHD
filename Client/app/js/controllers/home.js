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

        if(angular.isDefined(store.get('social'))) {
            if(store.get('social') == "google") {
                $scope.msg = "Đăng nhập google thành công";
                loginSocial('Google');
            }
            else
                $scope.msg = "Đăng nhập facebook thành công";
        }

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

        function loginSocial(provider) {
            $http({
                url: 'http://localhost:59219/api/Account/RegisterExternal',
                method: 'POST',
                data: {Provider: provider, ExternalAccessToken: store.get('accessToken').access_token}
            }).then(function(response) {
                store.set('dataSocial',response.data);
                console.log(response);
            }, function(error) {
                console.log(error);
            });
        }
    });

    return home;

});