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
        }
    });
}

function clear() {
    $('#txtarea-comment').val('');
    console.log('clicked');
}