/**
 * Created by ThaiSon on 14/12/2016.
 */

define(function (require) {
    'use strict';

    var contact = angular.module('contact', []);

    contact.controller('contact', function (NgMap, $scope, store, sharedData, $http) {
        NgMap.getMap().then(function(map) {
        });

        $scope.name = "";
        $scope.phone = "";
        $scope.email = "";
        $scope.message = "";

        $scope.send = function () {
          if($scope.name !== "" &&
            $scope.phone !== "" &&
            $scope.email !== "" &&
            $scope.message !== "")
              {
                  $http({
                      method: 'POST',
                      url: sharedData.host + '/api/Customer/SendMessage',
                      data: {
                          Username: $scope.name,
                          Message: $scope.message,
                          Phone: $scope.phone,
                          Email: $scope.email

                      }
                  }).then(function successCallback(response) {

                      console.log(response);
                  }, function errorCallback(response) {
                      console.log(response);
                  });
              }
        };
    });

    return contact;

});