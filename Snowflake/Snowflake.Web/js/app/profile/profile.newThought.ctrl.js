angular.module('app').controller('ProfileNewThoughtController', function ($scope, thoughtsResource) {
    $scope.createThought = function () {
        thoughtsResource.save($scope.newThought, function () {
            alert('Just another flake of snow on the ground!');
        });
    }
});