angular.module('app').controller('ConversationDetailController', function ($scope, $stateParams, conversationResource,  messagesResource, AuthenticationService) {
    conversationResource.get({ conversationId: $stateParams.conversationId }, function (response) {
        $scope.conversation = response;
        $scope.title = "Conversation between " + $scope.conversation.Users[0].UserName + ' and ' + $scope.conversation.Users[1].UserName;
        $scope.$apply();
    });
    $scope.username = AuthenticationService.state.username;
    $scope.sendMessage = function (message, username) {
        if (message && message !== '' && username) {
            messagesResource.save({
                ConversationId: $scope.conversation.ConversationId,
                Timestamp: new Date(),
                Text: message
            });
        }
    };
    
});