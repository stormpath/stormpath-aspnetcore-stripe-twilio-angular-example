(function () {
    'use strict';

    function dashboardService($http) {
        var addCredit = function (payload) {
            return $http.post('/api/payment', payload);
        };

        var loadData = function () {
            return $http.get('/api/me');
        };

        var sendBitcoinRate = function (payload) {
            return $http.post('/api/message', payload);
        };

        return {
            AddCredit: addCredit,
            LoadData: loadData,
            SendBitcoinRate: sendBitcoinRate
        };
    }

    angular.module('ToDoApp')
      .factory('DashboardService', ['$http', dashboardService])
}())