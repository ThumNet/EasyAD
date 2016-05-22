'use strict';
(function () {

    function easyADDashboardController($scope, $routeParams, $http, navigationService, treeService, easyADResource) {

        $scope.ready = false;

        easyADResource.getConfig().then(function (content) {
            $scope.ready = true;
            $scope.config = content.data;             
        });

    };

    //register the controller
    angular.module("umbraco").controller('EasyAD.DashboardController', easyADDashboardController);

})();