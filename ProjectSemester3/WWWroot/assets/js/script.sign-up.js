////$(document).ready(function () {

////    $('#sign-up').submit(function (e) {
////        e.preventDefault();
////        var name = $('#Name').val();
////        var idNum = $('#IdNum').val();
////        var email = $('#Email').val();
////        var phoneNumber = $('#PhoneNumber').val();
////        var job = $('#Job').val();
////        var address = $('#Address').val();
////        var username = $('#Username').val();
////        var password = $('#Password1').val();
////        var confirmPassword = $('#ConfirmPassword').val();

////        $(".error").remove();

        
////        if (name.length < 1) {
////            $('#Name').after('<span style="color:red;" class="error">This field is required</span>');
////        }
////        if (phoneNumber.length < 1) {
////            $('#PhoneNumber').after('<span class="error">This field is required</span>');
////        }
////        if (email.length < 1) {
////            $('#Email').after('<span style="color:red;" class="error">This field is required</span>');
////        } else {
////            var regEx = /^[A-Z0-9][A-Z0-9._%+-]{0,63}@(?:[A-Z0-9-]{1,63}\.){1,125}[A-Z]{2,63}$/;
////            var validEmail = regEx.test(email);
////            if (!validEmail) {
////                $('#Email').after('<span style="color:red;" class="error">Enter a valid email</span>');
////            }
////        }
////        if (password.length < 8) {
////            $('#Password1').after('<span style="color:red" class="error">Password must be at least 8 characters long</span>');
////        }
////        if (password != confirmPassword) {
////            $('#ConfirmPassword').after('<span style="color:red" class="error">Confirm password does not match</span>');
////        }
////    });

////    $('form[id=""]').validate({
////        rules: {
////            Name: 'required',
////            IdNum: 'required',
////            Email: {
////                required: true,
////                email: true,
////            },
////            PhoneNumber: {
////                required: true,
////                number:true,
////                minlength: 10,
////                maxlength:11,
////            },
////            Password: {
////                required: true,
////                minlength: 8,
////            }
////        },
////        messages: {
////            Name: 'This field is required1</span>',
////            lname: 'This field is required',
////            user_email: 'Enter a valid email',
////            psword: {
////                minlength: 'Password must be at least 8 characters long'
////            }
////        },
////        submitHandler: function (form) {
////            form.submit();
////        }
////    });

////});
$(function () {
    $(".datepicker").datepicker({
        dateFormat: 'dd/mm/yy',//check change
        changeMonth: true,
        changeYear: true
    });
    $("#Birthday").datepicker();
});