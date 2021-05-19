// convert datetime from database to normal dd/MM/yyyy date
function GetDate(_date) {
    let dateResults = _date.toString().split("T");
    var date = new Date(dateResults[0]);
    let result = '';
    result += (date.getDate() > 9) ? (date.getDate() + "/") : ("0" + date.getDate() + "/");
    result += (date.getMonth() + 1 > 9) ? (date.getMonth() + 1 + "/") : ("0" + (date.getMonth() + 1) + "/");
    result += date.getFullYear();
    return result;
}
