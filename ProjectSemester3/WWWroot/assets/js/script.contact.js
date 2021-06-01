function Save() {
    var question = document.getElementById("Question").value;

    var object = {
        "Question": question
    }
    $.post("save", object, function (data) {
        if (data.success) {
            Swal.fire(
                'Successfully',
                data.error,
                'success'
            );
            
        } else {
            Swal.fire(
                'Opp...',
                data.error,
                'error'
            );
            
        }
        ResetForm();
    }).fail(function (data) {
        Swal.fire(
            'Opp....',
            data.error,
            'error'
        );
       
    });
}
function ResetForm() {
    $("#Question").val("");
    $("#Subject").val("");
}