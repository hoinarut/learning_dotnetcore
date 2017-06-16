var app = angular.module("app-trips", ["simpleControls", "ngRoute"]);
app.config(function ($routeProvider) {
    $routeProvider.when("/", {
        template: "<trips></trips>"
    });
    $routeProvider.when("/editor/:tripName", {
        controller: "tripEditorController",
        controllerAs: "vm",
        templateUrl: "/views/tripEditorView.html"
    });
    $routeProvider.otherwise({ redirectTo: "/" });
});

