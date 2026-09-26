
var AddNewCommentTestData = function () {
    for (let i = 1; i <= 5; i++) {
        var Comment = {};
        var _Date = new Date();

        Comment.Id = i;
        Comment.DocumentId = 0;
        Comment.Message = "Auto Generated Comment at, " + _Date.toUTCString();
        Comment.CreatedBy = $("#CurrentUserId").val();
        listComment.push(Comment);
    }

    if (listComment.length > 0) {
        listComment.forEach(LoadTableRowFromDB);
    }

    $("#SL").val(6);
    $("#Name").val(GenRanString(10));
    $("#Notes").val(GenRanString(16));
    $('#CategoriesId').val(6);
    $('#DocumentStatus').val(1);
}

