angular.module('app').factory("userResource", function ($resource, apiUrl) {
    return $resource(apiUrl + 'user/:userId', { userId: '@UserId' },
    {
        'update': {
            method: 'PUT'
        }
    });
});