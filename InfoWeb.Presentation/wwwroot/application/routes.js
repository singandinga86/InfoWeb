function configureRoutes($stateProvider)
{
    /*$stateProvider.state('contacts', {
        templateUrl: "",
        controller: 'ContactsCtrl'
    });*/

    $stateProvider.state('default', {
        templateUrl: "application/views/login/login.html",
        controller: 'LoginController',
        url: ""
    });

    $stateProvider.state('login', {
        templateUrl: "application/views/login/login.html",
        controller: 'LoginController',
        url:"/login"
    });

    $stateProvider.state('projectList', {
        templateUrl: "application/views/project/list.html",
        controller: 'ProjectController',
        url: "/projects",
        authenticate: true
    });


}