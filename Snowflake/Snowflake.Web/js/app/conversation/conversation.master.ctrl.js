angular.module('app').controller('ConversationMasterController', function ($scope, conversationResource) {
    $scope.conversations = conversationResource.query();
});