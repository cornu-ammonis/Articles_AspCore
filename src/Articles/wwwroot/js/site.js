// Write your Javascript code.
function searchInitiate() {
    $("div#demoContainer").html("searching...");

}

function searchSuccess() {
    var sr = $(this).attr('data-str');
    $("div#statusResultContainer").html(sr);
    var s = $("#txtS").val();
    var link = "<a href=\"/Blog/Search?s=" + s + "\">Load Full Results</a>";
    $("div#fullListLink").html(link);

}

function savedFullLink() {
    $("div#fullListLink")
        .html("<a href=\"/Blog/SavedPosts\">Load All Saved Posts As Fresh Page</a>");
}

function hideUnsavedPost() {
    var postdivid = $(this).attr('data-postslug');
    var posttitle = $(this).attr('data-posttitle');
    $("div#" + postdivid).html("unsaved post titled: " + posttitle + "&nbsp");
}