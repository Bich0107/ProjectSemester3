function bankAccountChange() {
    $.ajax({
        type: 'GET',
        url: '/bank-account/getmodel/' + document.getElementById("list-code").value,
        contentType: 'application/json',
        dataType: 'json',
        success: function (model) {
            var result = '';
            result += '<tr>';
            result += '<td>Account Holder</td>';
            result += '<td class="text-right">' + model.userAccount.name + '</td>';
            result += '</tr>';
            result += '<tr>';
            result += '<td>Current Balance</td>';
            result += '<td class="text-right">' + model.balance;
            result += '<input type="hidden" id="balance" value="' + model.balance + '"/>';
            result += '</td>';
            result += '</tr>';            
            console.log(result);
            $('#bank-account-detail tbody').html(result);
        }
    });
};
function changeAccount() {
    $.ajax({
        type: 'GET',
        url: '/bank-account/getbankmodel/' + $("input#account-to").val(),
    }).done((model) => {
        if (model != null) {
            var result = '';
            result += '<tr>';
            result += '<td>Account Holder</td>';
            result += '<td class="text-right">' + model.userAccount.name;
            result += '</td>';
            result += '</tr>';
            result += '<tr>';
            result += '<td>Bank Name</td>';
            result += '<td class="text-right">' + model.bank.name + '</td>';
            result += '</tr>';
            console.log(result);
            $('#bank-account-to tbody').html(result);
            document.getElementById("account-failed").textContent = "";
        } else {
            var result = '';
            result += '<tr>';
            result += '</tr>';
            console.log(result);
            $('#bank-account-to tbody').html(result);
            document.getElementById("account-failed").textContent = "Account Invalid";
        }
    }).fail((err) => {
        console.log(err);        
    });
};
function validateForm() {
    var account = document.getElementById("account-to").value;
    var balance = document.getElementById("balance").value;
    var amount = document.getElementById("amount").value;
    var valid = document.getElementById("account-failed").innerHTML;
    //var balance = 0;
    if (account == "") {
        Swal.fire(
            'Invalid',
            'Please choose an account valid',
            'info'
        );
        return false;
    }
    if (account != "" && valid != "") {
        Swal.fire(
            'Invalid',
            'Please choose an account valid',
            'info'
        );
        return false
    }
    if (parseFloat(amount) > parseFloat(balance) || parseFloat(balance) == 0) {
        Swal.fire(
            'Invalid',
            'Your account balance is not enough to make a transaction',
            'info'
        );
        return false
    }
    if (parseFloat(amount) <= 0) {
        Swal.fire(
            'Invalid',
            'Please select the money you want to transfer',
            'info'
        );
        return false
    }
    
    
    return true;
}