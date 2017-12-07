var app = angular.module("InfoWeb");
app.factory("WorkedHoursService", ['$http', "UrlService",
    function ($http, UrlService) {
        var baseUrl = UrlService.getApiUrlPrefix();
        return {
            uploadHours: function (userId, workedHoursData) {
                return $http({
                    method: 'POST',
                    data: workedHoursData, 
                    url: baseUrl+ "user/" + userId + "/WorkedHours"
                });
            },
            getWorkedHours: function (user, searchValue) {
                //console.log(searchValue);
                var userId = user.id;
                var url = UrlService.getApiUrlPrefix() + "user/" + userId + "/WorkedHours/" +"?searchValue="+ searchValue;

                return $http({
                    method: 'GET',
                    url: url
                });
            }
        }
    }]);