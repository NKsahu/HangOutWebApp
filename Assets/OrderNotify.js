
function BycashStatusVerifiedCount() {
    $.ajax({
        url: "/WebApi/CountByCashUnverify",
        type: "GET",
        success: function (data) {
            var Jobj = JSON.parse(data);
            var Count = Jobj.Cnt;
            $("#CountPymAlt").text();
        },
        error: function (Xr, Status, ErrorMsg) {

        }
    });
}
function ShowPaymtAltdetails() {

}

setTimeout(function () { BycashStatusVerifiedCount() }, 1000);
setInterval(function () { BycashStatusVerifiedCount(); }, 40000);
