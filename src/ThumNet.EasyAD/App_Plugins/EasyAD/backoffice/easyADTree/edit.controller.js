'use strict';
(function () {
    
    function easyADEditController($scope, $routeParams, $http, $location, navigationService, notificationsService, easyADResource) {
        
        $scope.errorMessage = '';
        $scope.loaded = false;
        $scope.options = {};
        $scope.sections = {};

        easyADResource.getOptions().then(function (response) {
            $scope.options = response.data;
            createSections($scope.options.AllSections);
            loadGroup($routeParams.id);
        });

        $scope.save = function (group) {
            $scope.errorMessage = '';

            group.Sections = getGroupSections($scope.sections);
            easyADResource.save(group).then(function (response) {
                handleSaveResult(response.data);
            });
        };

        function createSections(allSections) {

            // create an easy object to use with the checkbox list for the available sections
            angular.forEach(allSections, function (value, key) {
                $scope.sections[key] = false;
            });
        }

        function loadGroup(groupId) {

            if (groupId == -1) {
                $scope.group = {};
                $scope.loaded = true;
            }
            else {
                //get a person id -> service
                easyADResource.getById(groupId).then(function (response) {
                    $scope.group = response.data;
                    checkGroupSections($scope.group.Sections);
                    $scope.loaded = true;
                });
            }
        }

        

        function checkGroupSections(groupSections) {

            // update the active sections for the current group.Sections
            angular.forEach(groupSections.split(';'), function (value, key) {
                if (value in $scope.sections) {
                    $scope.sections[value] = true;
                }
            });
        }

        function getGroupSections(sections) {

            // update the group.Sections to store the selected sections
            var groupSections = '';
            angular.forEach(sections, function (value, key) {
                if ($scope.sections[key]) {
                    groupSections += key + ';';
                }
            });
            return groupSections;
        }

        function handleSaveResult(result) {
            if (!result.Success) {
                $scope.errorMessage = result.Message;
                return;
            }

            $scope.easyADForm.$dirty = false;
            navigationService.syncTree({ tree: 'easyADTree', path: [-1, -1], forceReload: true });
            notificationsService.success("Success", "Group " + group.Name + " has been saved");

            if ($scope.group.Id !== result.GroupId) {
                // redirect to the created item
                $location.path('/users/easyADTree/edit/' + result.GroupId);
            }
        }

    };

    //register the controller
    angular.module("umbraco").controller('EasyAD.EditController', easyADEditController);

})();