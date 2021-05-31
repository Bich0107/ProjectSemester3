
function ShowUpdate(_id) {
    ResetUpdateForm();

    var url = "/profile/edit/" + _id;
    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        url: url,
        data: {
            id: _id
        },
        success: function (result) {
            console.log(result);
            $("#E-Id").val(_id);
            $("#E-Name").val(result.name);

            var birthday = new Date(result.birthday);
            var day = ("0" + birthday.getDate()).slice(-2);
            var month = ("0" + (birthday.getMonth() + 1)).slice(-2);
            var resultDate = birthday.getFullYear() + "-" + (month) + "-" + (day);

            $("#E-Birthday").val(resultDate);

            $("#E-PhoneNumber").val(result.phoneNumber);
            $("#E-Email").val(result.email);
            $("#E-Address").val(result.address);
            $("#E-Job").val(result.job);
            $("#E-Gender").val(result.gender);
            $("#E-IdNum").val(result.idNum);
        }
    });
}

function SaveTarget() {
    var id = $("#E-Id").val();
    var phoneNum = $("#E-PhoneNumber").val();
    var email = $("#E-Email").val();
    var address = $("#E-Address").val();
    var job = $("#E-Job").val();

    var object = {
        "Id": id,
        "PhoneNumber": phoneNum,
        "Email": email,
        "Address": address,
        "Job": job,
    }
    //$.ajax({
    //    url: 'edit',
    //    type: 'POST',
    //    data: object,
    //    dataType: 'json',
    //    success: function (data) {
    //        if (!data.success) {
    //            Swal.fire(
    //                'Opp...',
    //                data.error,
    //                'error'
    //            );
    //            $('#updateModal').modal('toggle');
    //        } else {
    //            Swal.fire(
    //                'Successfully!',
    //                data.Error,
    //                'success'
    //            );
    //            $('#updateModal').modal('toggle');
    //            LoadData(id);
    //        }

    //        LoadData();
    //    },
    //    error: function (data) {
    //        Swal.fire(
    //            'Opp....',
    //            'Something went wrong',
    //            'error'
    //        );
    //        $('#updateModal').modal('toggle');
    //        LoadData();
    //    }
    //});
    $.post("edit", object, function (data) {
        if (!data.success) {
            Swal.fire(
                'Opp...',
                data.error,
                'error'
            );
            $('#updateModal').modal('toggle');
        } else {
            Swal.fire(
                'Successfully!',
                data.error,
                'success'
            );
            $('#updateModal').modal('toggle');
            LoadData(id);
        }
        
        LoadData();
    }).fail(function (data) {
        Swal.fire(
            'Opp....',
            data.error,
            'error'
        );
        $('#updateModal').modal('toggle');
        LoadData();
    });
}
function LoadData(id) {
    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        url: 'getmodel/' + id,
        success: function (model) {
            var result = '';
            result += '<tr>';
            result += '<td>' + 'Name' + '</td>';
            result += '<td>' + model.name + '</td>';
            result += '</tr>';
            result += '<tr>';
            result += '<td>' + 'Birthday' + '</td>';
            result += '<td>' + GetDate(model.birthday) + '</td>';
            result += '</tr>';
            result += '<tr>';
            result += '<td>' + 'Phone Number' + '</td>';
            result += '<td>' + model.phoneNumber + '</td>';
            result += '</tr>';
            result += '<tr>';
            result += '<td>' + 'Email' + '</td>';
            result += '<td>' + model.email + '</td>';
            result += '</tr>';
            result += '<tr>';
            result += '<td>' + 'Address' + '</td>';
            result += '<td>' + model.address + '</td>';
            result += '</tr>';
            result += '<tr>';
            result += '<td>' + 'Job' + '</td>';
            result += '<td>' + model.job + '</td>';
            result += '</tr>';
            result += '<tr>';
            result += '<td>' + 'Gender' + '</td>';
            result += '<td>' + model.gender + '</td>';
            result += '</tr>';
            result += '<tr>';
            result += '<td>' + 'Id Number' + '</td>';
            result += '<td>' + model.idNum + '</td>';
            result += '</tr>';
            $('#account-object-table tbody').html(result);
        }
    })
}