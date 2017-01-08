/**
 * Created by ThaiSon on 24/12/2016.
 */

define(function (require) {
    'use strict';

    var payment = angular.module('payment', []);

    payment.controller('payment', function ($http, store, $scope, $state, Notification, sharedData) {
        $scope.userName = store.get('jwt').dbUser.name;
        $scope.district = "";
        $scope.phone = "";
        $scope.address = "";

        var listOrder = [];
        for(var i = 0; i < store.get('cart').length; i++)
        {
            listOrder.push({
                number: store.get('cart')[i].count,
                id: store.get('cart')[i].id
            });
        }

        $scope.sendOrder = function () {
            console.log(listOrder);
            if($scope.userName !== "" &&
            $scope.district !== "" &&
            $scope.phone !== "" &&
            $scope.address !== "") {
                $http({
                    method: 'POST',
                    url: sharedData.host + '/api/Customer/Order',
                    headers: {
                        Authorization: store.get('jwt').token
                    },
                    data: {
                        iduser: store.get('jwt').dbUser.id,
                        Username: $scope.userName,
                        Phone: $scope.phone,
                        District: "Hồ Chí Minh",
                        Ward: $scope.district.trim(),
                        Address: $scope.address,
                        Listoders: listOrder
                    }
                }).then(function successCallback(response) {
                    Notification.success({message: 'Cảm ơn bạn đã đặt hàng!', delay: 1500});
                    store.remove('cart');
                    store.remove('count');
                    console.log(response);
                }, function errorCallback(response) {
                    console.log(response);
                });
            }
            else
            {
                Notification.warning({message: 'Vui lòng nhập đủ thông tin!', delay: 1500});
            }
        };
    });

    return payment;

});