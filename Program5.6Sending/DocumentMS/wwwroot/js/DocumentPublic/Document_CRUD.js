
var Details = function (id) {
    var url = "/DocumentPublic/Details?id=" + id;
    $('#titleExtraBigModal').html("Document Details");
    loadExtraBigModal(url);
};

var ShareDocument = function (id) {
    var url = "/DocumentPublic/Details?id=" + id;
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