var Details = function (id) {
    var url = "/Comment/Details?id=" + id;
    $('#titleBigModal').html("Comment Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/Comment/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Comment");
    }
    else {
        $('#titleBigModal').html("Add Comment");
    }
    loadBigModal(url);
};

var Save = function () {
    if (!$("#frmComment").valid()) {
        return;
    }

    var _frmComment = $("#frmComment").serialize();
    $.ajax({
        type: "POST",
        url: "/Comment/AddEdit",
        data: _frmComment,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $('#tblComment').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    Swal.fire({
        title: 'Do you want to delete this item?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: "/Comment/Delete?id=" + id,
                success: function (result) {
                    var message = "Comment has been deleted successfully. Comment ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblComment').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};


$(() => {
    let connection = new signalR.HubConnectionBuilder().withUrl("/notify").build();
    connection.start();
    connection.on("refreshComment", function () {
        $('#tblComment').DataTable().ajax.reload();
    });
});