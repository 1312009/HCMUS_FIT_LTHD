/**
 * Created by ThaiSon on 01/12/2016.
 */

define(function (require) {
    'use strict';

    var email = angular.module('emailLogin', []);

    email.controller('emailLogin', function ($http, store, $scope, $state, $rootScope, sharedData, Notification) {
        $scope.user = {};
        $scope.forgetPass = sharedData.forgetPass;
        $scope.login = function () {
            $http({
                method: 'POST',
                url: sharedData.host + '/api/Account/signin',
                data: $scope.user
            }).then(function successCallback(response) {
                store.set('jwt', response.data);
                console.log(response);
                if(response.data.dbUser.name == "1312009") {
                    $rootScope.$emit("updateLogin", {});
                    $state.go("admin");
                }
                else {
                    $rootScope.$emit("updateLogin", {});
                    $state.go("/");
                }

            }, function errorCallback(response) {
                if(response.status == 500)
                {
                    Notification.error({message: 'Đăng nhập sai!', delay: 1500});
                }
                console.log(response);
            });
        };
    });

    return email;

});