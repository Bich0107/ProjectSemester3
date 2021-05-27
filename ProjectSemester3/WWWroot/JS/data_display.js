function LoadData() {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        url: 'loadData',
        success: function (result) {
            ShowValue(result);
        }
    })
}

function Search(keyword) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        url: 'search/' + keyword,
        success: function (result) {
            ShowValue(result);
        }
    })
}