﻿var module = angular.module("InfoWeb");

module.factory("UrlService", [function () {
    return {
        getServiceRootUrl: function ()
        {
            return "http://localhost:50367/";
        },
        getApiUrlPrefix: function ()
        {
            return "http://localhost:50367/api/";
        }
    }
}]);