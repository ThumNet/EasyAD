'use strict';
(function () {

    function easyADDeleteController($scope, $routeParams, $http, navigationService, treeService, easyADResource) {

        $scope.delete = function (id) {
            easyADResource.deleteById(id).then(function () {
                treeService.removeNode($scope.currentNode);
                navigationService.hideNavigation();
            });
        };
        
        $scope.cancelDelete = function () {
            navigationService.hideNavigation();
        };

    };

    //register the controller
    angular.module("umbraco").controller('EasyAD.DeleteController', easyADDeleteController);

})();