var module = angular.module("InfoWeb");

module.factory("NotificationService", ['$http', 'UrlService', function ($http, UrlService) {

    var currentUser = null;

    return {
        getNotifications: function (id) {
            var url = UrlService.getApiUrlPrefix() + 'Notification/' + id;
            return $http({
                method: 'GET',
                url: url
            });
        }
    }
}]);