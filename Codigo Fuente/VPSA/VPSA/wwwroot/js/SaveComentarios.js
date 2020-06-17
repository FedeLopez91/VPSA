$(function () {

    $("#SaveButton").click(function (e) {

        e.preventDefault();

        $.ajax({
            url: $("#NameView").attr("data-NombreView"), // Url
            data: {
                Nombre: $("#Nombre").val()
            },
            type: "POST"
        }).done(function (result) {
                getMessage(result);
        });
    });

    function getMessage(JsonMessage) {
        switch (JsonMessage.status) {
            case "OK":
                toastr.success(JsonMessage.message);
                $('#modal-lg').modal('hide');
                $("#Nombre").val("");
                location.reload();
                break;
            case "WARNING":
                toastr.warning(JsonMessage.message)
                break;
            case "ERROR":
                toastr.error(JsonMessage.message)
                break;
        }
    }

});