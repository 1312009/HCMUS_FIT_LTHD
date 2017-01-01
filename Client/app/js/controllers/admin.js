/**
 * Created by ThaiSon on 16/12/2016.
 */

define(function (require) {
    'use strict';

    var admin = angular.module('admin', []);

    admin.controller('admin', function ($http, $scope, store, sharedData, $timeout) {
        $timeout(function () {
            $scope.listFood = sharedData.listFood;
        }, 250);

        $scope.nameOfBtn = "Thêm món ăn";
        $scope.foodName = "";
        $scope.foodPrice = "";
        $scope.idType = null;
        $scope.isSale = null;
        $scope.foodImage = "";
        $scope.foodDescription = "";
        $scope.id = null;

        $scope.jwt = store.get('jwt');

        $scope.addFood = function () {
            if($scope.nameOfBtn == "Chỉnh sửa món ăn") {
                $http({
                    method: 'PUT',
                    url: 'http://localhost:59219/api/Admin/EditFood',
                    headers: {
                        Authorization: $scope.jwt.token
                    },
                    data: {
                        NAME: $scope.foodName,
                        DECRIPTION: $scope.foodDescription,
                        IDTYPE: parseInt($scope.idType),
                        NUMBER: 10,
                        IMGFOOD: $scope.foodImage,
                        PRICE: $scope.foodPrice,
                        ISSALE: $scope.isSale === true ? 1 : 0,
                        ID: $scope.id
                    }
                }).then(function successCallback(response) {
                    console.log(response.data);
                    updateItem($scope.id);
                    $scope.nameOfBtn = "Thêm món ăn";
                    $scope.foodName = "";
                    $scope.foodPrice = "";
                    $scope.idType = null;
                    $scope.isSale = null;
                    $scope.foodImage = "";
                    $scope.foodDescription = "";
                    $scope.id = null;
                }, function errorCallback(response) {
                    console.log(response);
                });
            }
            else {
                if(
                    $scope.foodName !== "" &&
                    $scope.foodPrice !== "" &&
                    $scope.idType !== null &&
                    $scope.foodImage !== "" &&
                    $scope.foodDescription !== ""
                )
                {
                    $http({
                        method: 'POST',
                        url: 'http://localhost:59219/api/Admin/AddFood',
                        headers: {
                            Authorization: $scope.jwt.token
                        },
                        data: {
                            NAME: $scope.foodName,
                            DECRIPTION: $scope.foodDescription,
                            IDTYPE: parseInt($scope.idType),
                            NUMBER: 10,
                            IMGFOOD: $scope.foodImage,
                            PRICE: $scope.foodPrice,
                            ISSALE: $scope.isSale === true ? 1 : 0
                        }
                    }).then(function successCallback(response) {
                        addItem();
                        $scope.nameOfBtn = "Thêm món ăn";
                        $scope.foodName = "";
                        $scope.foodPrice = "";
                        $scope.idType = null;
                        $scope.isSale = null;
                        $scope.foodImage = "";
                        $scope.foodDescription = "";
                        $scope.id = null;
                    }, function errorCallback(response) {
                        console.log(response);
                    });
                }
                else
                {
                    console.log("Vui long nhap day du du lieu!");
                }
            }
        };

        $scope.updateFood = function (food) {
                $scope.nameOfBtn = "Chỉnh sửa món ăn";
                $scope.idType = food.idtype.toString();
                $scope.foodName = food.name;
                $scope.foodPrice = food.price;
                $scope.foodImage = food.imgfood;
                $scope.id = food.id;
                $scope.foodDescription = food.decription;
                if (food.issale == 1)
                    $scope.isSale = true;
                else
                    $scope.isSale = false;

        };
        $scope.removeFood = function(food){
            $http({
                method: 'DELETE',
                url: 'http://localhost:59219/api/Admin/DeleteFood',
                headers: {
                    Authorization: $scope.jwt.token
                },
                params: {
                    ID: food.id
                }
            }).then(function successCallback(response) {
                console.log(response.data);
                removeItem(food.id);

            }, function errorCallback(response) {
                console.log(response);
            });
        };

        function addItem() {
            $http({
                url: "http://localhost:59219/api/foods/GetAllFoods",
                method: 'GET',
            }).then(function successCallback(response) {
                sharedData.listFood = response.data;
                $scope.listFood = sharedData.listFood;
                console.log(response);
            }, function errorCallback(error) {
                console.log(error);
            });
        }

        function updateItem(id) {
            for(var i = 0; i < $scope.listFood.length; i ++)
            {
                if($scope.listFood[i].id == id) {
                    $scope.listFood[i].name = $scope.foodName;
                    $scope.listFood[i].price = $scope.foodPrice;
                    $scope.listFood[i].idtype = parseInt($scope.idType);
                    $scope.listFood[i].decription = $scope.foodDescription;
                    $scope.listFood[i].imgfood = $scope.foodImage;
                    $scope.listFood[i].issale = $scope.isSale === true ? 1 : 0;
                }
            }
            console.log($scope.listFood);
        }

        function removeItem(id) {
            for(var i = 0; i < $scope.listFood.length; i ++)
            {
                if($scope.listFood[i].id == id) {
                    $scope.listFood.splice(i, 1);
                    console.log(i);
                }
            }
            console.log($scope.listFood);
        }

        $scope.getFoodType = function (type) {
            switch (type)
            {
                case 1:
                    return "Sáng";
                case 2:
                    return "Trưa";
                case 3:
                    return "Tối";
            }
        };

        $scope.getFoodState = function (n) {
            if(n > 0)
                return "Có bán";
            else
                return "Hết hàng";
        };
    });

    return admin;

});