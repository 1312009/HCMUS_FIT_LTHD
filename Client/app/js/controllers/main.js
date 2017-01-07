/**
 * Created by ThaiSon on 30/11/2016.
 */

define(['require',
    './account/social',
    './home',
    './menu',
    './reservation',
    './contact',
    './account/email',
    './account/register',
    './account/forgetPass',
    './account/changePass',
    './admin',
    './payment',
    './account/getToken'], function (require) {

    "use strict";

    var social = require('./account/social');
    var home = require('./home');
    var menu = require('./menu');
    var reservation = require('./reservation');
    var contact = require('./contact');
    var email = require('./account/email');
    var register = require('./account/register');
    var forgetPass = require('./account/forgetPass');
    var changePass = require('./account/changePass');
    var admin = require('./admin');
    var payment = require('./payment');
    var getToken = require('./account/getToken');

    var controllers = angular.module('controllers',['socialLogin',
                                                    'home',
                                                    'emailLogin',
                                                    'getToken',
                                                    'menu','reservation',
                                                    'contact',
                                                    'admin',
                                                    'payment',
                                                    'register',
                                                    'forgetPass',
                                                    'changePass']
    );

    return controllers;
});