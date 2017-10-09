var module = angular.module("InfoWeb");

module.factory("UrlService", [function () {
    return {
        getServiceRootUrl: function ()
        {
            return "http://localhost:50360/";
        },
        getApiUrlPrefix: function ()
        {
            return "http://localhost:50360/api/";
        }
    }
}]);