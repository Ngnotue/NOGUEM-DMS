$(document).ready(function () {
    document.title = 'Document History';

    $("#tblDocumentHistory").DataTable({
        paging: true,
        select: true,
        "order": [[0, "desc"]],
        dom: 'Bfrtip',


        buttons: [
            'pageLength',
        ],

        "processing": true,
        "serverSide": true,
        "filter": true, //Search Box
        "orderMulti": false,
        "stateSave": true,

        "ajax": {
            "url": "/DocumentHistory/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            {
                data: "Id", "name": "Id", render: function (data, type, row) {
                    return "<a href='#' onclick=Details('" + row.Id + "');>" + row.Id + "</a>";
                }
            },
            {
                data: "DocumentId", "name": "DocumentId", render: function (data, type, row) {
                    return "<a href='#' onclick=DocumentDetails('" + row.DocumentId + "');>" + row.DocumentId + "-Details</a>";
                }
            },
            { "data": "AssignEmployeeDisplay", "name": "AssignEmployeeDisplay" },
            { "data": "Action", "name": "Action" },
            { "data": "Note", "name": "Note" },
            { "data": "CreatedDateDisplay", "name": "CreatedDateDisplay" },
        ],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });

});

