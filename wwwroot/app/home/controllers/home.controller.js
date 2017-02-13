(function(){
  'use strict';
  
  function homeController(){
    var vm = this;

    vm.title = 'Home';

    return vm;
  }

  angular.module('ToDoApp')
    .controller('HomeController', [homeController]);
}())