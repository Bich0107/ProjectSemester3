$(document).ready(function () {


    $('#sign-up').validate({
        submitHandler: function (form) {
            AddCustomer();
        }
    });
});
function AddCustomer() {
    
    var name = $("#Name").val();
    var username = $("#Username").val();
    var birthday = $("#Birthday").val();
    var phoneNum = $("#PhoneNumber").val();
    var email = $("#Email").val();
    var address = $("#Address").val();
    var job = $("#Job").val();
    var gender = $("#Gender").val();
    var idNum = $("#IdNum").val();
    var password = $("#newPassword").val();

    var object = {
        "Name": name,
        "Username": username,
        "Birthday": birthday,
        "PhoneNumber": phoneNum,
        "Email": email,
        "Address": address,
        "Job": job,
        "Gender": gender,
        "IdNum": idNum,
        "Password": password,
    }


    $.post("sign-up", object, function (data) {
        if (data.success) {
            Swal.fire(
                'Successfully',
                data.error,
                'success'
            );
            if (data.result) {
                window.location = data.url;
            }
        } else {
            Swal.fire(
                'Opp...',
                data.error,
                'error'
            );

        }
        LoadData();
    });
    function LoadData() {
       
        $("#PhoneNumber").val("");
        $("#Email").val("");
        $("#Address").val("");
        $("#IdNum").val("");
        $("#newPassword").val("");
        $("#ConfirmPassword").val("");
    }
}

$(function () {
    $(".datepicker").datepicker({
        dateFormat: 'dd/mm/yy',//check change
        changeMonth: true,
        changeYear: true
    });
    $("#Birthday").datepicker();
});