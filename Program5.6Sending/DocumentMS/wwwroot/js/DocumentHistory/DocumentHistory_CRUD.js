var Details = function (id) {
    var url = "/DocumentHistory/Details?id=" + id;
    $('#titleMediumModal').html("Document History Details");
    loadMediumModal(url);
};

var DocumentDetails = function (id) {
    var url = "/Document/Details?id=" + id;
    $('#titleExtraBigModal').html("Document Details");
    loadExtraBigModal(url);
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
