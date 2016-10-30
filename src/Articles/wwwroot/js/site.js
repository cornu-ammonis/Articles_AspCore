// Write your Javascript code.
function sTest() {
    $("div#demoContainer").html("searching...");

}

function searchSuccess() {
    $("div#statusResultContainer").html("search completed");
    var s = $("#txtS").val();
    var link = "<a href=\"/Blog/Search?s=" + s + "\">Load Full Results</a>";
    $("div#fullListLink").html(link);

}

function savedFullLink() {
    $("div#fullListLink")
        .html("<a href=\"/Blog/SavedPosts\">Load All Saved Posts As Fresh Page</a>");
}