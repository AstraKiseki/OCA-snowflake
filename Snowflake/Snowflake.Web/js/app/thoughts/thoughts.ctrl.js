angular.module('app').controller('ThoughtsController', function ($scope, thoughtsResource, $http, apiUrl) {
    $scope.thoughts = thoughtsResource.query();

    $scope.like = function (thought) {
        $http.post(apiUrl + 'thoughts/' + thought.ThoughtId + '/like')
             .success(function () {
                 var index = $scope.thoughts.indexOf(thought);
                 $scope.thoughts.splice(index, 1);
             });
    }
    $scope.dislike = function (thought) {
        $http.post(apiUrl + 'thoughts/' + thought.ThoughtId + '/dislike')
             .success(function () {
                 var index = $scope.thoughts.indexOf(thought);
                 $scope.thoughts.splice(index, 1);
             });
    }
});