
var Details = function (id) {
    var url = "/Document/Details?id=" + id;
    $('#titleExtraBigModal').html("Document Details");
    loadExtraBigModal(url);
};

var ShareDocument = function (id) {
    var url = "/Document/Details?id=" + id;
    $('#titleExtraBigModal').html("Share Document");
    loadExtraBigModal(url);

    setTimeout(function () {
        activaTab('divShare');
        $('#ReceiverEmail').focus();
    }, 300);
};

var AssignEmployeeInfo = function (id) {
    if (id == 0) {
        FieldValidationAlert(null, 'This Document is not assigned yet to employees. ', "info");
    }
    else {
        var url = "/UserManagement/ViewUserDetails?Id=" + id;
        $('#titleBigModal').html("Employee Details");
        loadBigModal(url);
    }
};


var AddEdit = function (id) {
    var url = "/Document/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleExtraBigModal').html("Edit Document");
    }
    else {
        $('#titleExtraBigModal').html("Add Document");
    }
    loadExtraBigModal(url);
};

var Save = function () {
    if (!FieldValidation('#Name')) {
        FieldValidationAlert('#Name', 'Document Name is Required.', "warning");
        return;
    }
    if (!FieldValidation('#CategoriesId')) {
        FieldValidationAlert('#CategoriesId', 'Document Categories is Required.', "warning");
        return;
    }

    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');

    var _PreparedFormObj = PreparedFormObj();
    $.ajax({
        type: "POST",
        url: "/Document/AddEdit",
        data: _PreparedFormObj,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.OperationTyep == 1) {
                for (let i = 0; i < listComment.length; i++) {
                    listComment[i].Id = 0;
                    listComment[i].DocumentId = result.Id;
                }
                AddCommentList(listComment);
            }

            Swal.fire({
                title: result.AlertMessage,
                icon: "success"
            }).then(function () {
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                document.getElementById("btnClose").click();
                $('#tblDocument').DataTable().ajax.reload();
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
                url: "/Document/Delete?id=" + id,
                success: function (result) {
                    var message = "Document has been deleted successfully. Document ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblDocument').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};

var AddCommentList = function (listComment) {
    var SendObject = {
        listComment: listComment,
    };

    $.ajax({
        type: "POST",
        url: "/Comment/AddCommentList",
        data: SendObject,
        dataType: "json",
        success: function (result) {
            return result;
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var AddNewCommentDB = function () {
    if ($("#Message").val() === "" || $("#Message").val() === null) {
        Swal.fire({
            title: 'New comment field can not be null or empty.',
            icon: "warning",
            onAfterClose: () => {
                $("#Message").focus();
            }
        });
        return;
    }

    var CommentCRUDViewModel = {
        DocumentId: $("#Id").val(),
        Message: $("#Message").val(),
    };

    $.ajax({
        type: "POST",
        url: "/Comment/AddEdit",
        data: CommentCRUDViewModel,
        success: function (result) {
            $("#tblComment > tbody").empty();
            listComment.push(result);
            $("#Message").val("");
            if (result != null) {
                listComment.forEach(LoadTableRowFromDB);
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
};

var DeleteCommentDB = function (id) {
    $.ajax({
        type: "POST",
        url: "/Comment/Delete?id=" + id,
        success: function (result) {
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
};

var DocumentView = function () {
    var _IsFileSaveInDB = $("#IsFileSaveInDB").val();
    var _divFileDownload = document.getElementById("divFileDownload");
    var _divFileView = document.getElementById("divFileView");

    if (_IsFileSaveInDB == 'True') {
        _divFileDownload.style.display = "block";
        _divFileView.style.display = "none";
    }
    else {
        _divFileView.style.display = "block";
        _divFileDownload.style.display = "none";
    }
};


var PreparedFormObj = function () {
    var _FormData = new FormData()
    _FormData.append('Id', $("#Id").val())
    _FormData.append('CreatedDate', $("#CreatedDate").val())
    _FormData.append('CreatedBy', $("#CreatedBy").val())
    _FormData.append('Name', $("#Name").val())
    _FormData.append('CategoriesId', $("#CategoriesId").val())
    _FormData.append('DocumentStatus', $("#DocumentStatus").val())
    _FormData.append('AssignEmployeeId', $("#AssignEmployeeId").val())
    _FormData.append('Notes', $("#Notes").val())
    _FormData.append('FilesDirName', $("#FilesDirName").val())

    _FormData.append('Tag01', $("#Tag01").val())
    _FormData.append('Tag02', $("#Tag02").val())
    _FormData.append('Tag03', $("#Tag03").val())
    _FormData.append('Tag04', $("#Tag04").val())
    _FormData.append('Tag05', $("#Tag05").val())
    //_FormData.append('Files', $('#Files')[0].files[0])
    var fileUpload = $("#Files").get(0);
    var files = fileUpload.files;
    for (var i = 0; i < files.length; i++) {
        _FormData.append(files[i].name, files[i]);
    }

    if ($('#IsFileSaveInDB').is(":checked")) {
        _FormData.append('IsFileSaveInDB', true)
    }
    else
    {
        _FormData.append('IsFileSaveInDB', false)
    }
    return _FormData;
}