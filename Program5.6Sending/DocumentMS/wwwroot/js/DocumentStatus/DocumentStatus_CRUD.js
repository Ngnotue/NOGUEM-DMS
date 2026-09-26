var Details = function (id) {
    var url = "/DocumentStatus/Details?id=" + id;
    $('#titleBigModal').html("Document Status Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/DocumentStatus/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Document Status");
    }
    else {
        $('#titleBigModal').html("Add Document Status");
    }
    loadBigModal(url);
};

var Save = function () {
    if (!$("#frmDocumentStatus").valid()) {
        return;
    }

    var _frmDocumentStatus = $("#frmDocumentStatus").serialize();
    $.ajax({
        type: "POST",
        url: "/DocumentStatus/AddEdit",
        data: _frmDocumentStatus,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $('#tblDocumentStatus').DataTable().ajax.reload();
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
                url: "/DocumentStatus/Delete?id=" + id,
                success: function (result) {
                    var message = "Document Status has been deleted successfully. Document Status ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblDocumentStatus').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};
