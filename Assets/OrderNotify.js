$.notify.addStyle('foo', {
    html:
        "<div style='background-color:#9100ff' class='card'><div class='card-header text-center'> Orders</div><div class='card-body'><div class='row' id='NotificationList'>"
        + "<div class='col-sm-12 col-md-12 text-center'>"
        + "<h5>Screen3 B table2 Order No:4 TICKET NO: 4,5,6</h5>"
        + "<div><h5>Bill Amt : 1000 Rs/-</h5><button style='text-align:right' class='btn-info'>Details </button></div>"
        + "<h6>Order By Customer: NAVEEN(7389266697)</h5>"
        + "<h6>Cash By Customer</h5>"
        + "<div><button style='float:left' class='btn btn-info'>Not Received</button>"
        + "<button style='float:right' style='float:left' class='btn btn-info'>Received</button></div>"
        + "</div>"
        + "</div>"
        + "</div>"
        + "</div>"
});

function ShowNotice() {
    $.notify({
        title: 'Would you like some Foo ?',
        button: 'Confirm'
    }, {
            style: 'foo',
            autoHide: false,
            clickToHide: false
        });
}
//setTimeout(function () { ShowNotice() }, 500);

//listen for click events from this style
$(document).on('click', '.notifyjs-foo-base .no', function () {
    //programmatically trigger propogating hide event
    $(this).trigger('notify-hide');
});
$(document).on('click', '.notifyjs-foo-base .yes', function () {
    //show button text
    alert($(this).text() + " clicked!");
    //hide notification
    $(this).trigger('notify-hide');
});
