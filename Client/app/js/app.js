/**
 * Created by ThaiSon on 26/11/2016.
 */

define(function (require) {

    'use strict';

    var directives = require('./directives/main');

    var app = angular.module('foodOrder', [
        'ngAnimate',
        'directives',
        'ui.router'
    ]);

    app.init = function () {
        angular.bootstrap(document, ['foodOrder']);
    };

    // app.config([
    //     '$stateProvider',
    //     '$urlRouterProvider',
    //     '$locationProvider',
    //     function ($stateProvider, $urlRouterProvider, $locationProvider) {
    //         // $locationProvider.html5Mode(true);
    //         $urlRouterProvider.otherwise('/');
    //
    //         $stateProvider
    //
    //             .state("/", {
    //                 url: '/',
    //                 templateUrl: "/partials/bookForm.html",
    //                 controller: ""
    //             })
    //             .state("login", {
    //                 url: '/login',
    //                 templateUrl: "/pages/login.html",
    //                 controller: ""
    //             });
    //
    //     }]);

    return app;
});