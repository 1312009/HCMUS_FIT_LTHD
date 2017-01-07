/**
 * Created by ThaiSon on 06/12/2016.
 */

define(function (require) {
    'use strict';

    var sharedDT = angular.module('sharedData', []);

    sharedDT.factory("sharedData", [
        function(){
            var data = {
                listFood: '',
                meals: ["selected","",""],
                forgetPass: false,
                host: "http://localhost:59219",
                domain: "http://localhost:3000"
            };
            return data;
        }]);

    return sharedDT;

});