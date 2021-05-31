
function bankAccountChange() {
    $.ajax({
        type: 'GET',
        url: '/bank-account/getmodel/' + document.getElementById("list-code").value,
        contentType: 'application/json',
        dataType: 'json',
        success: function (model) {
            var result = '';
            result += '<tr>';
            result += '<td>Account Type</td>';
            result += '<td class="text-right">' + model.currency.name + '</td>';
            result += '</tr>';
            result += '<td>Current Balance</td>';
            result += '<td class="text-right">' + model.currency.name + ' ' + model.balance + '</td>';
            result += '</tr>';            
            console.log(result);
            $('#bank-account tbody').html(result);
        }
    });
};
function listTransaction() {
    var getSelectedValue = document.querySelector(
        'input[name="radio-transaction"]:checked'); 
    $.ajax({
        type: 'GET',
        url: '/bank-account/quick-search/' + document.getElementById("list-code").value + '/' + getSelectedValue.value,
        contentType: 'application/json',
        dataType: 'json',
        success: function (model) {
            var result = '';
            if (model == '') {
                result += '<p style="color:red;text-align:center">' + 'No transaction with this account' + '</p>';
            } else {
                for (var i = 0; i < model.length; i++) {
                    result += '<tr>';
                    result += '<td>' + model[i].time + '</td>';
                    result += '<td>' + model[i].content + '</td>';
                    result += '<td>' + model[i].amount + '</td>';
                    result += '<td class="text-right">' + model[i].balanceFrom + '</td>';
                    result += '</tr>';
                };
            }
            console.log(result);
            alert(document.getElementById("list-code").value);
            alert(getSelectedValue.value);
            $('#list-bank-account tbody').html(result);
        }
    });
};
function listTransactionByDate() {
    var getSelectedValue = document.querySelector(
        'input[name="radio-transaction"]:checked');
    $.ajax({
        type: 'GET',
        url: '/bank-account/quick-search/' + document.getElementById("list-code").value + '/' + getSelectedValue.value,
        contentType: 'application/json',
        dataType: 'json',
        success: function (model) {
            var result = '';
            if (model == '') {
                result += '<tr>';
                result += '<td>' + ' '+ '</td>';
                result += '<td class="text-right">' + '<p style="color:red;text-align:center">' + 'No transaction with this account' + '</p>' + '</td>';
                result += '<td>' + ' ' + '</td>';
                result += '</tr>';
            } else {
                for (var i = 0; i < model.length; i++) {
                    result += '<tr>';
                    result += '<td>' + model[i].time + '</td>';
                    result += '<td class="text-right">' + model[i].content + '</td>';
                    result += '<td>' + model[i].amount + '</td>';
                    result += '</tr>';
                };
            }
            console.log(result);
            alert(document.getElementById("list-code").value);
            alert(getSelectedValue.value);
            $('#list-bank-account tbody').html(result);
        }
    });
}
