angular.module('app', ['ngResource', 'ui.router', 'LocalStorageModule', 'irontec.simpleChat', 'ng-Aria']);

angular.module('app').value('apiUrl', 'http://localhost:54844/api/');

angular.module('app').config(function ($stateProvider, $urlRouterProvider, $httpProvider) {
    $httpProvider.interceptors.push('AuthenticationInterceptor');
    $urlRouterProvider.otherwise('home');
    $stateProvider
        .state('home', { url: '/home', templateUrl: '/templates/home/home.html', controller: 'HomeController' })
        .state('register', { url: '/register', templateUrl: '/templates/register/register.html', controller: 'RegisterController' })
        .state('login', { url: '/login', templateUrl: '/templates/login/login.html', controller: 'LoginController' })
        .state('credits', { url: '/credits', templateUrl: '/templates/credits/credits.html', controller: 'CreditsController'})
        .state('app', { url: '/app', templateUrl: '/templates/app/app.html', controller: 'AppController' })
                .state('app.conversation', { url: '/conversation', template: '<ui-view />', abstract: true })
                      .state('app.conversation.master', { url: '/master', templateUrl: '/templates/app/conversation/conversation.master.html', controller: 'ConversationMasterController' })
                      .state('app.conversation.detail', { url: '/detail/:conversationId', templateUrl: '/templates/app/conversation/conversation.detail.html', controller: 'ConversationDetailController' })
                .state('app.profile', { url: '/profile', template: '<ui-view />', abstract: true })
                      .state('app.profile.editThought', { url: '/editThought', templateUrl: '/templates/app/profile/profile.editThought.html', controller: 'ProfileEditThoughtController' })
                      .state('app.profile.newThought', { url: '/newThought', templateUrl: '/templates/app/profile/profile.newThought.html', controller: 'ProfileNewThoughtController' })
                      .state('app.profile.information', { url: '/information', templateUrl: '/templates/app/profile/profile.information.html', controller: 'ProfileInformationController' })
                      .state('app.profile.thoughts', { url: '/thoughts', templateUrl: '/templates/app/profile/profile.thoughts.html', controller: 'ProfileThoughtsController' })
                .state('app.thoughts', { url: '/thoughts', templateUrl: '/templates/app/thoughts/thoughts.html', controller: 'ThoughtsController'})
});

angular.module('app').run(function (AuthenticationService) {
    AuthenticationService.initialize();
});