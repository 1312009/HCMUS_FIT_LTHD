/**
 * Created by ThaiSon on 14/12/2016.
 */

define(function (require) {
    'use strict';

    var contact = angular.module('contact', []);

    contact.controller('contact', function (NgMap, $scope, store, sharedData, $http, Notification) {
        NgMap.getMap().then(function (map) {
        });

        $scope.name = "";
        $scope.phone = "";
        $scope.email = "";
        $scope.message = "";

        $scope.send = function () {
            if ($scope.name !== "" &&
                $scope.phone !== "" &&
                $scope.email !== "" &&
                $scope.message !== "") {
                if(!myForm.input.$valid)
                {
                    Notification.error({message: 'Email không đúng định dạng!', delay: 1500});
                }
                else {
                    $http({
                        method: 'POST',
                        url: sharedData.host + '/api/foods/SendMessage',
                        data: {
                            Username: $scope.name,
                            Message: $scope.message,
                            Phone: $scope.phone,
                            Email: $scope.email

                        }
                    }).then(function successCallback(response) {
                        $scope.name = "";
                        $scope.phone = "";
                        $scope.email = "";
                        $scope.message = "";
                        Notification.info({message: 'Cảm ơn phản hồi của bạn!', delay: 1500});
                        console.log(response);
                    }, function errorCallback(response) {
                        console.log(response);
                    });
                }
            }
            else
            {
                Notification.warning({message: 'Vui lòng nhập đầy đủ dữ liệu!', delay: 1500});
            }
        };

    });

    return contact;

});