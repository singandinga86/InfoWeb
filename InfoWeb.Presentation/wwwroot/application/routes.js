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

    $stateProvider.state('assign', {
        templateUrl: "application/views/project/assign.html",
        controller: 'ProjectAssignController',
        url: "/assign",
        authenticate: true
    });


    $stateProvider.state('delegateProject', {
        templateUrl: "application/views/project/delegate.html",
        controller: 'ProjectDelegateController',
        url: "/delegateProject",
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

    $stateProvider.state('projectDetails', {
        templateUrl: "application/views/project/details.html",
        controller: 'ProjectDetailsController',
        controllerAs: 'vm',
        url: "/projects/:projectId",
        authenticate: true
    }); 

    $stateProvider.state('assignHoursToDeveloper', {
        templateUrl: "application/views/assignments/single-developer.html",
        controller: 'DeveloperAssignmentController',
        url: "/assign/developer",
        authenticate: true
    }); 

    $stateProvider.state('hourTypeList', {
        templateUrl: "application/views/hour-type/list.html",
        controller: 'HourTypesController',
        url: "/hourtypes"
    }); 

    $stateProvider.state('createHourType', {
        templateUrl: "application/views/hour-type/hour-type-input.html",
        controller: 'CreateHourTypeController',
        url: "/hourtypes/create"
    }); 

    $stateProvider.state('updateHourType', {
        templateUrl: "application/views/hour-type/hour-type-input.html",
        controller: 'UpdateHourTypeController',
        url: "/hourtypes/:id/update"
    }); 

    $stateProvider.state('assignmentTypeList', {
        templateUrl: "application/views/assignment-type/list.html",
        controller: 'AssignmentTypeListController',
        url: "/assignmenttypes"
    });

    $stateProvider.state('createAssignmentType', {
        templateUrl: "application/views/assignment-type/assignment-type-input.html",
        controller: 'CreateAssignmentTypeController',
        url: "/hourtypes/create"
    });

    $stateProvider.state('updateAssignmentType', {
        templateUrl: "application/views/assignment-type/assignment-type-input.html",
        controller: 'UpdateAssignmentTypeController',
        url: "/hourtypes/:id/update"
    }); 

    /*$stateProvider.state('tree', {
        templateUrl: "application/views/tree/tree.html",
        controller: 'SampleController',
        controllerAs: 'vm',
        url: "/tree",
    }); */


}