/**
 * Created by ThaiSon on 07/01/2017.
 */

define(function (require) {
    'use strict';

    var changePass = angular.module('changePass', []);

    changePass.controller('changePass', function ($http, store, $scope, $state, $rootScope, sharedData, Notification) {
        $scope.email = "";
        $scope.oldPassword = "";
        $scope.newPassword = "";
        $scope.confirmPassword = "";

        $scope.change = function () {
            if($scope.email !== "" &&
                $scope.oldPassword !== "" &&
                $scope.newPassword !== "" &&
                $scope.confirmPassword !== "")
            {
                if($scope.newPassword !== $scope.confirmPassword)
                {
                    Notification.warning({message: 'Password và ConfirmPassword chưa chính xác!', delay: 1500});
                }
                else {
                    $http({
                        method: 'PUT',
                        url: sharedData.host + '/api/Account/ChangePassword',
                        data: {
                            Email: $scope.email,
                            OldPassword: $scope.oldPassword,
                            NewPassword: $scope.newPassword,
                            ConfirmPassword: $scope.confirmPassword
                        }
                    }).then(function successCallback(response) {
                        sharedData.forgetPass = false;
                        $state.go("login");
                        console.log(response);
                    }, function errorCallback(response) {
                        console.log(response);
                    });
                }
            }
        };
    });

    return changePass;

});