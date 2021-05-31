$(function () {
    $.validator.setDefaults({
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    })

    $.validator.addMethod("noSpace", function (value, element) {
        return this.optional(element) || /^[A-Za-z0-9]+(?: +[A-Za-z0-9]+)*$/.test(value);
    }, "This field can't have leading, trailing spaces, or special characters.");

    $.validator.addMethod("validatePassword", function (value, element) {
        return this.optional(element) || /^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%&]).*$/.test(value);
    }, "Password must have at least 8 characters, 1 number, 1 lowercase, 1 uppercase, and 1 special character from @#$%& ");

    $.validator.addMethod("minAge", function (value, element, min) {
        var today = new Date();
        var birthDate = new Date(value);
        var age = today.getFullYear() - birthDate.getFullYear();

        if (age > min + 1) {
            return true;
        }

        var m = today.getMonth() - birthDate.getMonth();

        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }

        return age >= min;
    }, "You are not old enough!");

    jQuery.extend(jQuery.validator.messages, {
        required: "This field is required.",
        remote: "Please fix this field.",
        email: "Please enter a valid email address.",
        url: "Please enter a valid URL.",
        date: "Please enter a valid date.",
        minAge: "You must be at least 18 years old!",
        dateISO: "Please enter a valid date (ISO).",
        number: "Please enter a valid number.",
        digits: "Please enter only digits.",
        creditcard: "Please enter a valid credit card number.",
        equalTo: "Please enter the same value again.",
        accept: "Please enter a value with a valid extension.",
        maxlength: jQuery.validator.format("Please enter no more than {0} characters."),
        minlength: jQuery.validator.format("Please enter at least {0} characters."),
        rangelength: jQuery.validator.format("Please enter a value between {0} and {1} characters long."),
        range: jQuery.validator.format("Please enter a value between {0} and {1}."),
        max: jQuery.validator.format("Please enter a value less than or equal to {0}."),
        min: jQuery.validator.format("Please enter a value greater than or equal to {0}.")
    });

    $.validator.addClassRules({
        title: {
            required: true,
            maxlength: 100,
            noSpace: true,
        },
        positiveNum: {
            required: true,
            min: 5,
        },
        confirmPassword: {
            required: true,
            equalTo: "#newPassword"
        },
        password: {
            required: true,
            validatePassword: true,
            maxlength: 20,
        },
        subject: {
            required: true,
            maxlength: 200,
            noSpace: true,
        },
        balance: {
            required: true,
            min: 0,
        },
        date: {
            required: true,
        },
        birthday: {
            required: true,
            minAge: 18
        },
        phoneNumber: {
            required: true,
            minlength: 9,
            maxlength: 13,
            digits: true,
            noSpace: true,
        },
        idNum: {
            required: true,
            minlength: 9,
            maxlength: 12,
            noSpace: true,
            digits: true,
        },
        email: {
            required: true,
            email: true,
        },
        name: {
            required: true,
            maxlength: 100,
            noSpace: true,
        },
        address: {
            required: true,
            minlength: 5,
            maxlength: 100,
            noSpace: true,
        },
        job: {
            required: true,
            minlength: 3,
            maxlength: 100,
            noSpace: true,
        },
        username: {
            required: true,
            minlength: 5,
            maxlength: 15,
            noSpace: true,
        },
        description: {
            required: true,
            noSpace: true,
            maxlength: 500,
        },
        fullname: {
            required: true,
            minlength: 3,
            maxlength: 100,
            noSpace: true,
        },
        exchangeRate: {
            required: true,
            min: 0,
        },
    })

    $('#updateForm').validate({
        submitHandler: function (form) {
            SaveTarget();
        }
    });

    $('#addForm').validate({
        submitHandler: function (form) {
            CreateTarget();
        }
    });
})

function ResetAddForm() {
    // plug-in reset function
    $("#addForm").validate().resetForm();

    // fix bootstrap reset error
    $('#addForm .form-control').removeClass('is-invalid');

    $("#addForm").trigger("reset");
}

function ResetUpdateForm() {
    // plug-in reset function
    $("#updateForm").validate().resetForm();

    // fix bootstrap reset error
    $('#updateForm .form-control').removeClass('is-invalid');
}