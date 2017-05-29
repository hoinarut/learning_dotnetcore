// Write your Javascript code.
// site.js
(function () {
    var $sidebarAndWrapper = $("#sidebar,#wrapper");

    $("#menuToggle").on("click", function () {
        $sidebarAndWrapper.toggleClass("display-sidebar");
        if ($sidebarAndWrapper.hasClass("display-sidebar")) {
            $(this).text("Show Menu");
        } else {
            $(this).text("Hide Menu");
        }
    });

})();