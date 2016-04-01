angular.module('app').factory("messagesResource", function ($resource, apiUrl) {
    return $resource(apiUrl + 'messages/:messageId', { messageId: '@MessageId' },
    {
        'update': {
            method: 'PUT'
        }
    });
});