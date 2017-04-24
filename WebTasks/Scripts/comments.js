function post(task_id) {
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
            Content: txt
        },
        success: (result) => {
            $("#comments-panel").after(result);
            $('#txtarea-comment').val('');
        }
    });
}

function clearTxtArea() {
    $('#txtarea-comment').val('');
    console.log('clicked');
}

function deleteTask(id) {
    $.ajax({
        method: "DELETE",
        url: "/Comments/Delete",
        data: {
            TaskId: id
        },
        statusCode: {
            200: () => {
                $("#comment-" + id).remove();
            },
            401: () => {
                // TODO: use noty
                alert("You are Unauthorized to perform this action");
            }
        }
    })
}