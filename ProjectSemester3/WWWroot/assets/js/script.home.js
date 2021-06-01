$(function () {
    $(".datepicker").datepicker({
        dateFormat: 'dd/mm/yy',//check change
        changeMonth: true,
        changeYear: true
    });
    $("#CreatedDate").datepicker();
    $("#CreatedDate").datepicker('setDate', new Date());
    var expiredDate = new Date();
    expiredDate.setFullYear(expiredDate.getFullYear() + 5, expiredDate.getMonth(), expiredDate.getDate());
    $("#ExpiredDate").datepicker('setDate', expiredDate).attr('readonly', 'readonly');
});
function listTransaction2(id) {
    $.ajax({
        type: 'GET',
        url: '/home/statement/' + id,
        contentType: 'application/json',
        dataType: 'json',
        success: function (model) {
            var result = '';
            if (model == '') {
                result += '<tr>';
                result += '<td>' + ' ' + '</td>';
                result += '<td class="text-right">' + '<p style="color:red;text-align:center">' + 'No transaction with this account' + '</p>' + '</td>';
                result += '<td>' + ' ' + '</td>';
                result += '</tr>';
            } else {
                for (var i = 0; i < model.length; i++) {                    
                    result += '<tr>';
                    result += '<td>' + model[i].time + '</td>';
                    result += '<td>' + model[i].content + '</td>';
                    result += '<td>' + model[i].amount + '</td>';
                    result += '<td class="text-right">' + model[i].balanceFrom + '</td>';
                    result += '</tr>';
                };
                result += '<input type="hidden" name="id" value="' + id + '"/>';
            }
            console.log(result);
            $('#list-bank-account tbody').html(result);
        }
    });
};