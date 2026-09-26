var AddNewCustomer = function () {
    activaTab('divAddNewCustomer');
};

var SaveCustomerInfo = function () {
    if (!FieldValidation('#Name')) {
        FieldValidationAlert('#Name', 'Name is Required.', "warning");
        return;
    }

    var CustomerInfoCRUDViewModel = {
        Name: $("#Name").val(),
        Phone: $("#Phone").val(),
        Email: $("#Email").val(),
        BillingAddress: $("textarea#BillingAddress").val(),
    };

    $.ajax({
        type: "POST",
        url: "/CustomerInfo/AddEdit",
        data: CustomerInfoCRUDViewModel,
        success: function (result) {
            $('#CustomerId').append($('<option>', {
                value: result.Id,
                text: result.Name
            }));
            $('#CustomerId').val(result.Id);
            toastr.success(result.AlertMessage, 'Success');
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
    activaTab('divBasicInfo');
};



