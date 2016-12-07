/**
 * Created by ThaiSon on 26/11/2016.
 */

define(function (require) {

    'use strict';

    var directives = require('./directives/main');
    var controllers = require('./controllers/main');
    var services = require('./services/main');

    var app = angular.module('foodOrder', [
        'ngAnimate',
        'directives',
        'controllers',
        'services',
        'ui.router',
        'angular-storage'
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

                .state("/", {
                    url: '/',
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

    app.filter('lengthstring', function () {
        return function (item) {
            if(item.length > 82)
            {
                return item.substr(0, 82) + '...'
            }else{
                return item
            }

        };
    });

    return app;
});