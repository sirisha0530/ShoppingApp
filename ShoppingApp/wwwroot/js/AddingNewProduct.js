function SubmitLogin() {
    var customer = {
        ProductName: $("#ProductName").val(),
        ProductDiscription: $("#ProductDiscription").val(),

        ProductPrice: $("#ProductPrice").val(),
        ProductImage: $("#ProductImage").val()

    };

    $.ajax
        ({
            type: 'POST',
            dataType: 'JSON',
            data: customer,
            url: "/Dashboard/GetProduct",
            success: function (result) {
                if (result == true) {
                    alert("logged in successfully");
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}