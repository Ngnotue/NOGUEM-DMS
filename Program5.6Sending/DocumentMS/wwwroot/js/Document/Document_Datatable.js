$(document).ready(function () {
    LoadDataTables();
});


var CustomRangeDataFilter = function () {
    var _StartDate = $("#StartDate").val();
    var _EndDate = $("#EndDate").val();
    location.href = "/Document/Index?StartDate= " + _StartDate + "&EndDate= " + _EndDate;
};

var LoadDataTables = function () {
    document.title = 'Document';
    $("#tblDocument").DataTable({
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
                "data": "CreatedDate",
                "name": "CreatedDate",
                "autoWidth": true,
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='fa fa-share' onclick=ShareDocument('" + row.Id + "');>Share</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-info btn-xs' onclick=AddEdit('" + row.Id + "');>Edit</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger btn-xs' onclick=Delete('" + row.Id + "'); >Delete</a>";
                }
            }
        ],

        'columnDefs': [{
            'targets': [5, 6],
            'orderable': false,
        }],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });
};
