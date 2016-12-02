/**
 * Created by ThaiSon on 01/12/2016.
 */

define(function (require) {
    'use strict';

    var email = angular.module('emailLogin', []);

    email.controller('emailLogin', function ($http, store, $scope, $state) {
        $scope.user = {};
        $scope.login = function () {

            $http({
                method: 'POST',
                url: 'http://localhost:59219/api/Account/signin',
                data: $scope.user
            }).then(function successCallback(response) {
                store.set('jwt', response.data.token);
                console.log(response);

                $state.go("home");
            }, function errorCallback(response) {
                console.log(response);
                console.log($scope.user);
            });
        };
    });

    return email;

});