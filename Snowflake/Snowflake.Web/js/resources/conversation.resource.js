angular.module('app').factory("conversationResource", function ($resource, apiUrl) {
    return $resource(apiUrl + 'conversations/:conversationId', { conversationId: '@ConversationId' },
    {
        'update': {
            method: 'PUT'
        }
    });
});