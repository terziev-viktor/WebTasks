let currentPage = 1;

function nextPage() {
    currentPage++;

    $("#tb-container").slideUp("fast", () => {
        $.ajax({
            method: "GET",
            url: "DailyTasks/Page/" + currentPage,
            success: (partial) => {
                $("#tb-container").html(partial);
                $("#tb-container").slideDown("fast");
            }
        });
    });
}

function previousPage() {
    currentPage--;
    if (currentPage < 1) {
        currentPage = 1;
        return;
    }

    $("#tb-container").slideUp("fast", () => {
        $.ajax({
            method: "GET",
            url: "DailyTasks/Page/" + currentPage,
            success: (partial) => {
                $("#tb-container").html(partial);
                $("#tb-container").slideDown("fast");
            }
        });
    });
}