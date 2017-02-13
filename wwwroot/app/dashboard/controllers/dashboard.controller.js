(function () {
    'use strict';

    function dashboardController(DashboardService, $mdDialog, $scope, $timeout) {
        var vm = this;

        vm.loadData = function () {
            DashboardService.LoadData().then(
                function (results) {
                    vm.totalQueries = results.data.totalQueries;
                    vm.balance = results.data.balance;
                    vm.apiKeyId = results.data.apiKeyId;
                    vm.apiKeySecret = results.data.apiKeySecret;
                }, errorHandler);
        };

        vm.showPaymentForm = function (ev) {
            $mdDialog.show({
                controller: paymentFormController,
                templateUrl: 'app/dashboard/templates/payment-form.tmpl.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                fullscreen: false
            })
        };

        vm.sendBitcoinExchangeRate = function () {
            vm.sendingSMS = true

            DashboardService.SendBitcoinRate({ phoneNumber: vm.phoneNumber }).then(
                function (response) {
                    vm.sendingSMS = false
                    vm.totalQueries = response.data.totalQueries;
                    vm.balance = response.data.balance;
                    vm.phoneNumber = '';

                    $mdDialog.show(
                        $mdDialog.alert()
                            .parent(angular.element(document.querySelector('body')))
                            .clickOutsideToClose(true)
                            .title('Success!')
                            .textContent('SMS sent successfully.')
                            .ariaLabel('SMS Success')
                            .ok('Ok')
                    );
                }, function (error) {
                    vm.sendingSMS = false
                    $mdDialog.show(
                        $mdDialog.alert()
                            .parent(angular.element(document.querySelector('body')))
                            .clickOutsideToClose(true)
                            .title('Error')
                            .textContent('An error occurred while sending the SMS. Please try again.')
                            .ariaLabel('SMS Error')
                            .ok('Ok')
                    );
                }
            );
        };

        function paymentFormController($scope) {
            $scope.value = 'VALUE';

            $scope.closePaymentFormModal = function () {
                $mdDialog.hide();
            }

            $scope.stripeCallback = function (code, result) {
                if (result.error) {
                    $mdDialog.show(
                        $mdDialog.alert()
                            .parent(angular.element(document.querySelector('body')))
                            .clickOutsideToClose(true)
                            .title('Error')
                            .textContent('An error occurred while retrieving the payment token from Stripe. Please try again.')
                            .ariaLabel('Payment Error')
                            .ok('Ok')
                    );
                } else {
                    $scope.processingPayment = true;
                    DashboardService.AddCredit({ Token: result.id })
                        .then(function (response) {
                            $scope.processingPayment = false;
                            vm.totalQueries = response.data.totalQueries;
                            vm.balance = response.data.balance;
                            $mdDialog.hide();
                        }, function (error) {
                            $scope.processingPayment = false;
                            $mdDialog.show(
                                $mdDialog.alert()
                                    .parent(angular.element(document.querySelector('body')))
                                    .clickOutsideToClose(true)
                                    .title('Error')
                                    .textContent('An error occurred while processing your payment. Please try again.')
                                    .ariaLabel('Payment Error')
                                    .ok('Ok')
                            );
                        })
                }
            };
        }

        return vm;
    }

    function errorHandler(error) {
        console.error(error);
    }

    angular.module('ToDoApp')
      .controller('DashboardController', ['DashboardService', '$mdDialog', '$scope', '$timeout', dashboardController]);
}())
