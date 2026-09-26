var Details = function (id) {
    var url = "/DocumentCategories/Details?id=" + id;
    $('#titleBigModal').html("Document Categories Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/DocumentCategories/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Document Categories");
    }
    else {
        $('#titleBigModal').html("Add Document Categories");
    }
    loadBigModal(url);
};

var Save = function () {
    if (!$("#frmDocumentCategories").valid()) {
        return;
    }

    var _frmDocumentCategories = $("#frmDocumentCategories").serialize();
    $.ajax({
        type: "POST",
        url: "/DocumentCategories/AddEdit",
        data: _frmDocumentCategories,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $('#tblDocumentCategories').DataTable().ajax.reload();
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
                url: "/DocumentCategories/Delete?id=" + id,
                success: function (result) {
                    var message = "Document Categories has been deleted successfully. Document Categories ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblDocumentCategories').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};
