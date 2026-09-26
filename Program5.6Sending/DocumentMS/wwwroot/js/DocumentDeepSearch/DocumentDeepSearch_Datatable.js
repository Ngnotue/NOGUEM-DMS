$(document).ready(function () {
    document.title = 'Document Deep Search';
    LoadDataTable();
});

var LoadDataTable = function () {
    $("#tblDocumentDeepSearch").DataTable({
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
            "url": "/Document/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            { "data": "Id", "name": "Id" },
            {
                data: "Name", "name": "Name", render: function (data, type, row) {
                    return "<a href='#' onclick=Details('" + row.Id + "');>" + row.Name + "</a>";
                }
            },
            { "data": "CategoriesDisplay", "name": "CategoriesDisplay" },
            {
                data: "AssignEmployeeDisplay", "name": "AssignEmployeeDisplay", render: function (data, type, row) {
                    return "<a href='#' class='fa fa-eye' onclick=AssignEmployeeInfo('" + row.AssignEmployeeId + "');>" + row.AssignEmployeeDisplay + "</a>";
                }
            },
            { "data": "DocumentStatusDisplay", "name": "DocumentStatusDisplay" },

            {
                "data": "CreatedDate", "name": "CreatedDate", render: function (data, type, row) {
                    return ConvertDateToDDMMYYYY(row.CreatedDate);
                }
            }
        ],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });
}

