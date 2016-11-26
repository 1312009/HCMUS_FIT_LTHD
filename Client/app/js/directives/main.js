/**
 * Created by ThaiSon on 26/11/2016.
 */

define(function (require) {
    "use strict";

    var header = require('./myHeader/myHeader');
    var footer = require('./myFooter/myFooter');

    var directives = angular.module('directives',['header','footer']);

    return directives;
});