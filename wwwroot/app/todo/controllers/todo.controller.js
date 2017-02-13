(function(){
  'use strict';
  
  function todoController(TodoService){
    var vm = this;
    vm.title = "To Do";

    vm.loadTodos = function(){
      TodoService.Get().then(
        function(results){
          vm.todos = results.data;
        },
        errorHandler);
    };

    vm.addTodo = function(){
      if(vm.newTodo.description){
        TodoService.Save(vm.newTodo).then(
          function(result){
            vm.todos.push(result.data);
            vm.newTodo = { }
          },
          errorHandler)
      }
    };

    vm.markComplete = function(todo){
      if(todo){
        TodoService.Save(todo).then(
          function(result){ 
            // do nothing 
          },
          errorHandler)
      }
    };

    vm.deleteTodo = function(todo){
      if(todo){
        TodoService.Delete(todo.id).then(
          function(result){
            vm.todos.splice(vm.todos.indexOf(todo), 1);
          },
          errorHandler)
      }
    };

    return vm;
  }

  function errorHandler(error){
    console.error(error);
  }

  angular.module('ToDoApp')
    .controller('TodoController', ['TodoService', todoController]);
}())