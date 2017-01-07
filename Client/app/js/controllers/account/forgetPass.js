/**
 * Created by ThaiSon on 07/01/2017.
 */

define(function (require) {
    'use strict';

    var forgetPass = angular.module('forgetPass', []);

    forgetPass.controller('forgetPass', function ($http, store, $scope, $state, $rootScope, sharedData, Notification) {

        $scope.email = "";

        $scope.register = function () {
            if($scope.email !== "")
            {
                if($scope.email === "")
                {
                    Notification.warning({message: 'Vui lòng nhập email!', delay: 1500});
                }
                else {
                    $http({
                        method: 'GET',
                        url: sharedData.host + '/api/Account/ForgetPassword',
                        params: {
                            email: $scope.email
                        }
                    }).then(function successCallback(response) {
                        sharedData.forgetPass = true;
                        $state.go("login");
                        console.log(response);
                    }, function errorCallback(response) {
                        console.log(response);
                    });
                }
            }
        };
    });

    return forgetPass;

});