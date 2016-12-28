/**
 * Created by ThaiSon on 30/11/2016.
 */

define(function (require) {
    'use strict';

    var social = angular.module('socialLogin', []);

    social.controller('socialLogin', function ($http, $scope, store, $rootScope, sharedData) {

        store.remove('cart');
        store.remove('count');
        console.log("social");
        callApi('Secured', 'http://localhost:59219/api/foods/GetAllFoods');


        function callApi(type, url) {
            // $scope.response = null;
            $scope.api = type;
            $http({
                url: url,
                method: 'GET',
            }).then(function(response) {
                sharedData.listFood = response.data;
                console.log(sharedData.listFood);
            }, function(error) {
                console.log(error);
            });
        }

        $scope.loginGG = function () {
            var client_id = "872912626455-bvlpomh5rsnccib0of29qjfj9o4u59ir.apps.googleusercontent.com";
            var scope = "email";
            var redirect_uri = "http://localhost:3000";
            var response_type = "token";
            var url = "https://accounts.google.com/o/oauth2/auth?scope=" + scope + "&client_id=" + client_id + "&redirect_uri=" + redirect_uri +
                "&response_type=" + response_type;
            store.set('social','google');
            console.log("gg login");
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

        $scope.isLogin = false;
        if(angular.isDefined(store.get('social')) && store.get('social') !== null)
        {
            $scope.isLogin = true;
            if(store.get('social') == "google")
                loginSocial('Google');
            else
                loginSocial('Facebook');
        }

        if(angular.isDefined(store.get('jwt')) && store.get('jwt') !== null)
        {
            $scope.isLogin = true;
            $scope.user = store.get('jwt').dbUser;
        }

        function loginSocial(provider) {
            $http({
                url: 'http://localhost:59219/api/Account/RegisterExternal',
                method: 'POST',
                data: {Provider: provider, ExternalAccessToken: store.get('accessToken').access_token}
            }).then(function(response) {
                store.set('jwt',response.data);
                $scope.user = response.data.dbUser;
                console.log(response);
            }, function(error) {
                console.log(error);
            });
        }

        $scope.reset = function () {
            store.remove('social');
            store.remove('accessToken');
            store.remove('jwt');
            store.remove('cart');
        };

        $scope.cart = 0;
        if(angular.isDefined(store.get('cart')) && store.get('cart') !== null)
        {
            $scope.cart = store.get('count');
        }

        $rootScope.$on("updateCart", function(){
            updateCart();
        });

        var updateCart = function () {
            $scope.cart = store.get('count');
        };

        //set current menu
        $scope.currentMenu = ["current", "", "", ""];

        if(window.location.hash === "#/menu")
            $scope.currentMenu = ["", "current", "", ""];
        if(window.location.hash === "#/reservation")
            $scope.currentMenu = ["", "", "current", ""];
        if(window.location.hash === "#/contact")
            $scope.currentMenu = ["", "", "", "current"];

        $scope.changeCurrentMenu = function (id) {
            for(var i = 0; i < 4; i++)
            {
                if(i === id)
                    $scope.currentMenu[i] = "current";
                else
                    $scope.currentMenu[i] = "";
            }
        };

        $rootScope.$on("setMenu", function(event, id){
            $scope.changeCurrentMenu(id);
        });

    });

    return social;

});
