function findProfilesUser() {
    let email = $("#inp-email-search").val(),
        username = $("#inp-username-search").val();

    reqData = {
        Email: email,
        UserName: username
    }

    $.ajax({
        method: "POST",
        data: {
            Email: email,
            UserName = username
        },
        url: "/Search/SearchProfiles",
        success: (data, arg1) => {
            console.log(data);
            console.log(arg1);
            $("#found-profiles-container").html(data);
        }
    });
}