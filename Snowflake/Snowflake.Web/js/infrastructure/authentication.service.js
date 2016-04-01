angular.module('app').factory('AuthenticationService', function ($http, $q, localStorageService, apiUrl) {
    var state = {
        authorized: true
    };

    function initialize() {
        var token = localStorageService.get('token');

        if (token) {
            state.authorized = true;
            state.username = localStorageService.get('username').username;
        }
    }

    function register(registration) {
        logout();
        return $http.post(apiUrl + 'accounts/register', registration).then(
            function (response) {
                return response;
            }
        );
    }

    function login(loginData) {
        var data = 'grant_type=password&username=' + loginData.username + '&password=' + loginData.password;

        var deferred = $q.defer();

        logout();

        $http.post(apiUrl + 'token', data, {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        }).success(function (response) {
            localStorageService.set('token', {
                token: response.access_token
            });
            localStorageService.set('username', {
                username: loginData.username
            });

            state.authorized = true;
            state.username = loginData.username;

            deferred.resolve(response);
        }).error(function (err, status) {
            logout();

            deferred.reject(err);
        });

        return deferred.promise;
    }

    function logout() {
        localStorageService.remove('token');

        state.authorized = false;

        return $q.resolve();
    }

    return {
        state: state,
        initialize: initialize,
        register: register,
        login: login,
        logout: logout
    };
});