/**
 * Created by ThaiSon on 30/11/2016.
 */

define(function (require) {
    "use strict";

    var social = require('./login/social');
    var home = require('./home');
    var menu = require('./menu');
    var reservation = require('./reservation');
    var contact = require('./contact');
    var email = require('./email');
    var getToken = require('./login/getToken');

    var controllers = angular.module('controllers',['socialLogin','home','emailLogin','getToken','menu','reservation','contact']);

    return controllers;
});