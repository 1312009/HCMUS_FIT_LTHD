/**
 * Created by ThaiSon on 30/11/2016.
 */

define(function (require) {
    'use strict';

    var getTK = angular.module('getToken', []);

    getTK.controller('getToken', function ($location, store, $state) {

        var hash = $location.path().substr(1);

        var splitted = hash.split('&');
        var params = {};

        for (var i = 0; i < splitted.length; i++) {
            var param  = splitted[i].split('=');
            var key    = param[0];
            var value  = param[1];
            params[key] = value;
            store.set('accessToken', params);
        }

        $state.go("home");
    });

    return getTK;

});
