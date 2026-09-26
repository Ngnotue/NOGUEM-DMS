var Details = function (id) {
    var url = "/EmailTemplate/Details?id=" + id;
    $('#titleBigModal').html("Email Template Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/EmailTemplate/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Email Template");
    }
    else {
        $('#titleBigModal').html("Add Email Template");
    }
    loadBigModal(url);
};

var Save = function () {
    if (!$("#frmEmailTemplate").valid()) {
        return;
    }

    var _frmEmailTemplate = $("#frmEmailTemplate").serialize();
    $.ajax({
        type: "POST",
        url: "/EmailTemplate/AddEdit",
        data: _frmEmailTemplate,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $('#tblEmailTemplate').DataTable().ajax.reload();
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
                url: "/EmailTemplate/Delete?id=" + id,
                success: function (result) {
                    var message = "Email Template has been deleted successfully. Email Template ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblEmailTemplate').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};
