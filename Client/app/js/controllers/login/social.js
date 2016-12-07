/**
 * Created by ThaiSon on 30/11/2016.
 */

define(function (require) {
    'use strict';

    var social = angular.module('socialLogin', []);

    social.controller('socialLogin', function ($scope, store) {

        $scope.loginGG = function () {
            var client_id = "872912626455-bvlpomh5rsnccib0of29qjfj9o4u59ir.apps.googleusercontent.com";
            var scope = "email";
            var redirect_uri = "http://localhost:3000";
            var response_type = "token";
            var url = "https://accounts.google.com/o/oauth2/auth?scope=" + scope + "&client_id=" + client_id + "&redirect_uri=" + redirect_uri +
                "&response_type=" + response_type;
            store.set('social','google');
            window.location.replace(url);
        };

        $scope.loginFB = function () {
            var client_id = "243001122781125";
            var scope = "email";
            var redirect_uri = "http://localhost:3000";
            var response_type = "token";
            var url = "https://www.facebook.com/dialog/oauth?scope=" + scope + "&client_id=" + client_id + "&redirect_uri=" + redirect_uri +
                "&response_type=" + response_type;
            store.set('social','facebook');
            window.location.replace(url);
        };

        $scope.isLogin = angular.isDefined(store.get('social'));
        if($scope.isLogin)
        {
            $scope.user = store.get('dataSocial').dbUser;
            console.log($scope.user);
        }


    });

    return social;

});
