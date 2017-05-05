function postComment(task_id, task_type) {
    let txt = $('#txtarea-comment').val();
    if (txt === null) {
        return;
    }
    txt = txt.trim();
    if (txt.lenght === 0) {
        return;
    }

    $.ajax({
        method: "POST",
        url: "/Comments/Create",
        data: {
            ForTask: task_id,
            TaskType: task_type,
            Content: txt
        },
        success: (result) => {
            if (result)
            {
                $("#comments-panel").after(result);
                $('#txtarea-comment').val('');
                noty({
                    text: "Comment posted successfully!",
                    type: 'success',
                    layout: 'topCenter',
                    timeout: 1000
                });
            }
        }, statusCode: {
            403: () => {
                noty({
                    text: "You cant post an empty comment.",
                    type: 'warning',
                    layout: 'topCenter',
                    timeout: 1000
                });
            },
            500: (err) => {
                console.log(err);

                noty({
                    text: "An error has accured while processing your request",
                    type: 'warning',
                    layout: 'topCenter',
                    timeout: 1000
                });
            }
        }
    });
}

function clearTxtArea() {
    $('#txtarea-comment').val('');
}