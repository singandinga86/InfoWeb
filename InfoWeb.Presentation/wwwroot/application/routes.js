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

    $stateProvider.state('userList', {
        templateUrl: "application/views/user/list.html",
        controller: 'UserListController',
        url: "/users",
        authenticate: true
    });

    $stateProvider.state('roleList', {
        templateUrl: "application/views/role/list.html",
        controller: 'RoleListController',
        url: "/roles",
        authenticate: true
    });
    $stateProvider.state('createRole', {
        templateUrl: "application/views/role/role-input.html",
        controller: 'CreateRoleController',
        url: "/roles/create",
        authenticate: true
    });
    $stateProvider.state('updateRole', {
        templateUrl: "application/views/role/role-input.html",
        controller: 'UpdateRoleController',
        url: "/roles/update/:roleId",
        authenticate: true
    });


}