(function(){
  'use strict';
  function config($stateProvider, $urlRouterProvider, $locationProvider, STORMPATH_CONFIG) {
      window.Stripe.setPublishableKey('YOUR_STRIPE_PUBLIC_KEY');
      $locationProvider.html5Mode(true);
      STORMPATH_CONFIG.FORM_CONTENT_TYPE = 'application/json';

      $stateProvider
        .state('home', {
            url: '/',
            templateUrl: '/app/home/views/home.view.html'
        })
        .state('register', {
            url: '/register',
            templateUrl: '/app/auth/views/register.view.html'
        })
        .state('login', {
            url: '/login',
            templateUrl: '/app/auth/views/login.view.html'
        })
        .state('dashboard', {
            url: '/dashboard',
            templateUrl: '/app/dashboard/views/dashboard.view.html',
            sp: {
                authenticate: true
            }
        })
        .state('todo', {
            url: '/todo',
            templateUrl: '/app/todo/views/todo.view.html',
            sp: {
                authenticate: true
            }
        });

      $urlRouterProvider.otherwise('/');
  }

  function initializer($stormpath, $rootScope, $state) {
      // Finally, configure the login state and the default state after login
      $stormpath.uiRouter({
          loginState: 'login',
          defaultPostLoginState: 'todo'
      });

      // Bind the logout event
      $rootScope.$on('$sessionEnd', function () {
          $state.transitionTo('login');
      });
  }

    angular.module('ToDoApp', ['ngCookies', 'ngResource', 'ngSanitize', 'ui.router', 'stormpath', 'stormpath.templates', 'ngMaterial', 'angularPayments'])
    .config(['$stateProvider', '$urlRouterProvider', '$locationProvider', 'STORMPATH_CONFIG', config])
    .run(['$stormpath', '$rootScope', '$state', initializer]);
}())