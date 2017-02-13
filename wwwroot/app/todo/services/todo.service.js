(function(){
  'use strict';

  function todoService($http){
    var get = function(){
          return $http.get('/api/todos');
        },
        save = function(todo){
          return $http.post('/api/todos', todo);
        },
        remove = function(id){
          return $http.delete('/api/todos/' + id);
        };

    return {
      Get: get,
      Save: save,
      Delete: remove
    };
  }

  angular.module('ToDoApp')
    .factory('TodoService', ['$http', todoService])
}())