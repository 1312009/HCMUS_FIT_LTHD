/**
 * Created by ThaiSon on 01/01/2017.
 */

define(function (require) {
    'use strict';

    var register = angular.module('register', []);

    register.controller('register', function ($http, store, $scope, $state, $rootScope, sharedData, Notification) {
        $scope.name = "";
        $scope.password = "";
        $scope.confirmPassword = "";
        $scope.email = "";

        $scope.register = function () {
            if($scope.name !== "" &&
                $scope.password !== "" &&
                $scope.confirmPassword !== "" &&
                $scope.email !== "")
            {
                if($scope.password !== $scope.confirmPassword)
                {
                    Notification.warning({message: 'Password và ConfirmPassword chưa chính xác!', delay: 1500});
                }
                else {
                    $http({
                        method: 'POST',
                        url: sharedData.host + '/api/Account/SignUp',
                        data: {
                            Username: $scope.name,
                            Email: $scope.email,
                            Password: $scope.password,
                            ConfirmPassword: $scope.confirmPassword

                        }
                    }).then(function successCallback(response) {

                        console.log(response);
                    }, function errorCallback(response) {
                        console.log(response);
                    });
                }
            }
        };
    });

    return register;

});