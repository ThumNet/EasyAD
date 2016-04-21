﻿'use strict';
(function () {

    function easyADResourceFactory($http) {
        var apiUrl = '/umbraco/backoffice/EasyAD/EasyADApi/';
        return {
            getById: function (id) {
                return $http.get(apiUrl + 'GetById?id=' + id);
            },
            getOptions: function () {
                return $http.get(apiUrl + 'GetOptions');
            },
            save: function (group) {
                return $http.post(apiUrl + 'PostSave', angular.toJson(group));
            },
            //save: function (person) {
            //    return $http.post("backoffice/Example/PersonApi/PostSave", angular.toJson(person));
            //},
            deleteById: function(id) {
                return $http.delete(apiUrl + 'DeleteById?id=' + id);
            }
        };
    };
    //register the controller
    angular.module("umbraco.resources").factory('easyADResource', easyADResourceFactory);

})();