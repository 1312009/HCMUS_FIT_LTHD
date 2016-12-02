/**
 * Created by ThaiSon on 30/11/2016.
 */

define(function (require) {
    'use strict';

    var social = angular.module('socialLogin', []);

    social.controller('socialLogin', function ($scope, store) {

        $scope.loginGG = function() {
                 	var client_id="579123578196-mq4oubjc4pl2jn2n4pkbrra46gt3994a.apps.googleusercontent.com";
                 	var scope="email";
                 	var redirect_uri="http://localhost:3000";
                 	var response_type="token";
                 	var url="https://accounts.google.com/o/oauth2/auth?scope="+scope+"&client_id="+client_id+"&redirect_uri="+redirect_uri+
                     	"&response_type="+response_type;
                 	window.location.replace(url);
                 };

        $scope.loginFB = function() {
            var client_id="243001122781125";
            var scope="email";
            var redirect_uri="http://localhost:3000";
            var response_type="token";
            var url="https://www.facebook.com/dialog/oauth?scope="+scope+"&client_id="+client_id+"&redirect_uri="+redirect_uri+
                "&response_type="+response_type;
            window.location.replace(url);
        };
    });

    return social;

});
