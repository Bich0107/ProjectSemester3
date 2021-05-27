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