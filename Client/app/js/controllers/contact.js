/**
 * Created by ThaiSon on 14/12/2016.
 */

define(function (require) {
    'use strict';

    var contact = angular.module('contact', []);

    contact.controller('contact', function (NgMap, $scope, store, sharedData) {
        NgMap.getMap().then(function(map) {
            console.log(map.getCenter());
            console.log('markers', map.markers);
            console.log('shapes', map.shapes);
        });
    });

    return contact;

});