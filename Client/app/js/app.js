/**
 * Created by ThaiSon on 26/11/2016.
 */

define(function (require) {

    'use strict';

    var directives = require('./directives/main');
    var controllers = require('./controllers/main');

    var app = angular.module('foodOrder', [
        'ngAnimate',
        'directives',
        'controllers',
        'ui.router'
    ]);

    app.init = function () {
        angular.bootstrap(document, ['foodOrder']);
    };

    app.config([
        '$stateProvider',
        '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            // $locationProvider.html5Mode(true);
            $urlRouterProvider.otherwise('/');

            $stateProvider

                // .state("/", {
                //     url: '/',
                //     templateUrl: "/partials/bookForm.html",
                //     controller: ""
                // })
                .state("home", {
                    url: '/home',
                    templateUrl: "/pages/home.html",
                    controller: "home"
                })
                .state("login", {
                    url: '/login',
                    templateUrl: "/pages/login.html",
                    controller: "emailLogin"
                })
                .state('get_token', {
                    url: '/access_token=:accessToken',
                    template: '',
                    controller: "getToken"
                });

        }]);

    return app;
});