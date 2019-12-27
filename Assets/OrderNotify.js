﻿
function BycashStatusVerifiedCount() {
    $.ajax({
        url: "/WebApi/CountByCashUnverify",
        type: "GET",
        success: function (data) {
            var Jobj = JSON.parse(data);
            var Count = Jobj.Cnt;
            $("#CountPymAlt").text(Count);
        },
        error: function (Xr, Status, ErrorMsg) {

        }
    });
}
function ShowPaymtAltdetails() {
    $('#waiting').show();
    $.ajax({
        url: "/HG_Orders/ByCashAlert",
        type: "GET",
        success: function (data) {
            makedpt("ByCashAlrt",500,500);
            $("#ByCashAlrt > .modal-dialog > .modal-content > .modal-body").html(data);
            showdpt("ByCashAlrt");
            $('#waiting').hide();

        },
        error: function (Xr, Status, ErrorMsg) {
            $('#waiting').hide();
        }
    });
}

setTimeout(function () { BycashStatusVerifiedCount() }, 1000);
setInterval(function () { BycashStatusVerifiedCount(); }, 40000);
function ShowDetails(OID) {
    //HG_OrderItem
    //OID
    $("#waiting").show();
    $.ajax({
        type: 'POST',
        url: "/HG_OrderItem/index?OID=" + OID,
        success: function (data) {
            makedpt("CurrOrder");
            $("#CurrOrder > .modal-dialog > .modal-content > .modal-body").html(data);
            showdpt("CurrOrder");
            $("#waiting").hide();
        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback
            $("#waiting").hide();
        }
    });

}