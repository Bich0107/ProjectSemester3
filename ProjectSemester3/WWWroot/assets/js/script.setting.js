$(document).ready(function () {
    

    $('#changePasswordForm').validate({
        submitHandler: function (form) {
            ChangePass();
        }
    });
    $('#A-UpdateForm').validate({
        submitHandler: function (form) {
            ChangeAuthenticator();
        }
    });
});
function ChangePass() {
    var id = $('#SettingId').val();
    var password = $('#oldPassword').val();
    var newPassword = $('#newPassword').val();

    var object = {
        "Id": id,
        "Password": password,
    }
    var newObject = {
        "Id": id,
        "Password": newPassword,
    }
    

    $.post("confirm-password", object, function (data) {
        if (data.success) {
            $.post("change-password", newObject, function (data) {
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
        } else {
            Swal.fire(
                'Opp...',
                data.error,
                'error'
            );
            
        }
        LoadData();
    });
}
function ChangeAuthenticator() {
    var code = $('#A-Code').val();

    $.post("authentication/" + code, code, function (data) {
        console.log(code);
        if (data.success) {
            Swal.fire(
                'Successfully',
                data.error,
                'success',
                '70000'
            );
            $('#updateAuthentication').modal('toggle');
            if (data.result) {             
                window.location = data.url;
            }            
        } else {
            Swal.fire(
                'Opp...',
                data.error,
                'error'
            );
            $('#updateAuthentication').modal('toggle');
            $('#updateAuthentication').prop('checked',data.check);

        }
        LoadData();
    });
}
function LoadData() {
    $('#oldPassword').val("");
    $('#newPassword').val("");
    $('#confirmNewPassword').val("");
    $('#A-Code').val("");
    
    
}