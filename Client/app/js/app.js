/**
 * Created by ThaiSon on 26/11/2016.
 */

define(['require', './directives/main', './controllers/main', './services/main'],function (require) {

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
        'angular-storage',
        'ngMap',
        'ui-notification'
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
                .state("register", {
                    url: '/register',
                    templateUrl: "/pages/register.html",
                    controller: "register"
                })
                .state("menu", {
                    url: '/menu',
                    templateUrl: "/pages/menu.html",
                    controller: "menu"
                })
                .state("reservation", {
                    url: '/reservation',
                    templateUrl: "/pages/reservation.html",
                    controller: "reservation"
                })
                .state("contact", {
                    url: '/contact',
                    templateUrl: "/pages/contact.html",
                    controller: "contact"
                })
                .state("admin", {
                    url: '/admin',
                    templateUrl: "/pages/admin.html",
                    controller: "admin",
                    data: {
                        requiresLogin: true
                    }
                })
                .state("payment", {
                    url: '/payment',
                    templateUrl: "/pages/payment.html",
                    controller: "payment",
                    data: {
                        requiresLogin: true
                    }
                })
                .state('get_token', {
                    url: '/access_token=:accessToken',
                    template: '',
                    controller: "getToken"
                })
                .state('reset', {
                    url: '/reset',
                    template: '',
                    controller: function(store, $state){
                        store.remove('social');
                        store.remove('accessToken');
                        store.remove('jwt');
                        store.remove('cart');
                        $state.go("/");
                        window.location.reload();
                    }
                });

        }]);

    app.filter('lengthstring', function () {
        return function (item) {
            if(item.length > 82)
            {
                return item.substr(0, 82) + '...';
            }else{
                return item;
            }

        };
    });

    app.run(function($rootScope, $state, store) {
        $rootScope.$on('$stateChangeStart', function(e, to) {
            if (to.data && to.data.requiresLogin) {
                if (!store.get('jwt')) {
                    e.preventDefault();
                    $state.go('login');
                }
            }
        });
    });

    return app;
});