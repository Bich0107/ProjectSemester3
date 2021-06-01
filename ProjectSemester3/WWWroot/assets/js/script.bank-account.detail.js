window.onload = function selectChange() {
    $('.form-control option[value=@b.Id]').attr('selected', 'selected');
};
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
            result += '<td>Current Balance</td>';
            result += '<td class="text-right">' + model.balance + ' ' + model.currency.name + '</td>';
            result += '</tr>';
            result += '<td>Currency Type</td>';
            result += '<td class="text-right">' + model.currency.name + '</td>';
            result += '</tr>';
            result += '<td>Account open date</td>';
            result += '<td class="text-right">' + GetDate(model.createdDate) + '</td>';
            result += '</tr>';
            result += '<td>Account expired date</td>';
            result += '<td class="text-right">' + GetDate(model.expiredDate) + '</td>';
            result += '</tr>';
            console.log(result);
            $('#bank-account-detail tbody').html(result);
        }
    });
}