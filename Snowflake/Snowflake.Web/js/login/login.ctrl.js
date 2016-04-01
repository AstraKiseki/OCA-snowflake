angular.module('app').controller('LoginController', function ($scope, AuthenticationService) {
    $scope.loginData = {};

    $scope.login = function () {
        AuthenticationService.login($scope.loginData).then(
            function (response) {
                location.replace('/#/app/thoughts');
            },
            function (err) {
                alert(err.error_description);
            }
        );
    };
    $scope.logout = function () {
        AuthenticationService.logout();
    }
});