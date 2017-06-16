angular.module("app-trips").component("trips", {
    templateUrl: '/views/tripsView.html',
    bindings: {

    },
    controller: function ($http) {
        this.trips = [];

        this.newTrip = {};
        this.errorMessage = "";
        this.isBusy = true;

        $http.get("/api/trips")
            .then(function (response) {
                angular.copy(response.data, this.trips);
            }, function (error) {
                this.errorMessage = "Failed to load data: " + error;
            })
            .finally(function () {
                this.isBusy = false;
            });

        this.addTrip = function () {
            this.isBusy = true;
            this.errorMessage = "";
            $http.post("/api/trips", this.newTrip)
                .then(function (response) {
                    this.trips.push(response.data);
                    this.newTrip = {};
                }, function (error) {
                    this.errorMessage = "Failed to save new trip";
                })
                .finally(function () {
                    this.isBusy = false;
                });
        };
    }
});