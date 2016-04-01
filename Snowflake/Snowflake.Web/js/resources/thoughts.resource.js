angular.module('app').factory("thoughtsResource", function ($resource, apiUrl) {
    return $resource(apiUrl + 'thoughts/:thoughtId', { thoughtId: '@ThoughtId' },
    {
        'update': {
            method: 'PUT'
        }
    });
});