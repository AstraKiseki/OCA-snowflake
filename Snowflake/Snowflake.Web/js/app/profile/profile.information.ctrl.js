angular.module('app').controller('ProfileInformationController', function ($scope, AuthenticationService) {
    $scope.username = AuthenticationService.state.username;
});