
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
    resetList();
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
                result += '<td>' + ' ' + '</td>';
                result += '<td class="text-right">' + '<p style="color:red;text-align:center">' + 'No transaction with this account' + '</p>' + '</td>';
                result += '<td>' + ' ' + '</td>';
                result += '</tr>';
            } else {
                for (var i = 0; i < model.length; i++) {
                    if (model[i].content == null) {
                        model[i].content == ' ';
                    };
                    result += '<tr>';
                    result += '<td>' + model[i].time + '</td>';
                    result += '<td>' + model[i].content + '</td>';
                    result += '<td>' + model[i].amount + '</td>';
                    result += '<td class="text-right">' + model[i].balanceFrom + '</td>';                    
                    result += '</tr>';
                };
                result += '<input type="hidden" name="id" value="' + document.getElementById("list-code").value + '"/>';
                result += '<input type="hidden" name="value" value="' + getSelectedValue.value + '"/>';
            }
            console.log(result);
            $('#list-bank-account tbody').html(result);
        }
    });
};
function listTransactionByDate() {
    resetList2();
    var id = document.getElementById("list-code").value; 
    var from = $("#From").val();
    var to = $("#To").val();
    var min = $("#Min").val();
    var max = $("#Max").val();
    if (from == "") {
        from = "2000-1-1";
    }
    if (to == "") {
        to = "2100-1-1";
    }
    if (min == "") {
        min = 0;
    }
    if (max == "") {
        max = 0;
    }
    alert(from + '/' + to + '/' + min + '/'+  max);
    $.ajax({
        type: 'GET',
        url: '/bank-account/search/' + id + '/' + from + '/' + to + '/' + min + '/' + max,
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
                    if (model[i].content == null) {
                        model[i].content == ' ';
                    };
                    result += '<tr>';
                    result += '<td>' + model[i].time + '</td>';
                    result += '<td>' + model[i].content + '</td>';
                    result += '<td>' + model[i].amount + '</td>';
                    result += '<td class="text-right">' + model[i].balanceFrom + '</td>';
                    result += '</tr>';
                };
                result += '<input type="hidden" name="id" value="' + document.getElementById("list-code").value + '"/>';
                result += '<input type="hidden" name="from" value="' + from + '"/>';
                result += '<input type="hidden" name="to" value="' + to + '"/>';
                result += '<input type="hidden" name="min" value="' + min + '"/>';
                result += '<input type="hidden" name="max" value="' + max + '"/>';
                
            }
            console.log(result);
            $('#list-bank-account2 tbody').html(result);
        }
    });
};
function resetList() {
    var result = '';
    $('#list-bank-account tbody').html(result);
};
function resetList2() {
    var result = '';
    $('#list-bank-account2 tbody').html(result);
};
