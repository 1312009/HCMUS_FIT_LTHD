/**
 * Created by ThaiSon on 26/11/2016.
 */

require.config({
    paths: {
        angular: '../bower_components/angular/angular'
    },
    shim: {
        "angular": {
            exports: "angular"
        }
    },
    baseUrl: '/js'

});

require(['app'], function (app) {
    app.init();
});