angular.module('app').controller('RegisterController', function ($scope, AuthenticationService) {
    $scope.registration = {};

    $scope.register = function () {
        AuthenticationService.register($scope.registration).then(
            function (response) {
                alert("Registration complete.");
                $scope.registration = {};
            },
            function (error) {
                alert("Failed to register");
            }
        )
    };
});