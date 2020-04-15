
window.addEventListener("submit", function (e) {
    var form = e.target;
    $("#waiting").show();
    //if (form.getAttribute("enctype") === "multipart/form-data") {
        if (form.dataset.ajax) {
            e.preventDefault();
            e.stopImmediatePropagation();
            var xhr = new XMLHttpRequest();
            xhr.open(form.method, form.action);
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    console.log("AAYA");
                    var JObj = "";
                    try {
                        JObj = JSON.parse(xhr.responseText);

                    } catch (e) {
                        //alert(e); // error in the above string (in this case, yes)!
                    }

                    if (JObj.msg != undefined && JObj.msg != "") {
                        Onfail(JObj.msg);
                        
                    }
                    else if (JObj.url != undefined) {
                        window.location.href = JObj.url;
                        $("#waiting").hide();
                    }
                    else if (form.dataset.ajaxUpdate) {
                        var updateTarget = document.querySelector(form.dataset.ajaxUpdate);
                        if (updateTarget) {
                            updateTarget.innerHTML = xhr.responseText;
                            $("#waiting").hide();
                            $('#CreateModel').modal('toggle');
                        }
                    }
                    else if (JObj!=null) {
                        JsonSuccess(JObj);
                    }
                }
            };
            xhr.send(new FormData(form));
        }
    //}
}, true);



function Onfail(msg) {
                      $("#waiting").hide();
                         $("#WarningModel .modal-body").html(msg);
                        $("#WarningModel").modal();
}
//shortcut.add("Esc", function () {
//    e.preventDefault();
//    return;
//});
function HideModal(event) {
    $(event).parent('div').parent('div').parent('div').parent('div').hide();
    $(event).parent('div').parent('div').parent('div').parent('div').remove();
    console.log("AAYA" + $(".modal:visible").length);
    if ($(".modal:visible").length == 0) {
        $('body').removeClass('modal-open');
        $('body').removeAttr('style');
        $('.modal-backdrop').remove();
    }
    
}

function makedpt(id, h, w) {
    cloned = $('#myModal');
    $("#" + id).remove();
    $('#myModal').clone().attr('id', id).insertBefore(cloned);
    $(".close").click(function (event) {
        $(this).parent(".modal-header").parent(".modal").hide();
        $(this).parent(".modal-header").parent(".modal-content").parent(".modal-dialog").parent(".modal").remove();
    });
    $(".closeBtn").click(function (event) {
        $(this).parent(".modal-footer").parent(".modal").hide();
        $(this).parent(".modal-footer").parent(".modal-content").parent(".modal-dialog").parent(".modal").remove();
    });
    if (h != null) {
        $("#" + id + " > .modal-dialog").css('max-height', h);
        $("#" + id + " > .modal-dialog > .modal-content").css('max-height', h);
        $("#" + id + " > .modal-dialog").css('min-height', h);
        $("#" + id + " > .modal-dialog > .modal-content").css('min-height', h);
        $("#" + id + " > .modal-dialog").removeClass('modal-dialog-max');
    }
    if (w != null) {
        $("#" + id + " > .modal-dialog").css('max-width', w);
        $("#" + id + " > .modal-dialog").css('margin', "");
    }
    
}
function showdpt(id) {
   modaladded = true;
   // window.location.hash = id;
    $("#" + id).show();
    
   // $("#" + id).display = "block";
    $("#" + id).modal({ backdrop: 'static', keyboard: false });
    $("#" + id).css('padding-right', '0px');

}
function hidedpt(id) {
    if ($(".modal:visible").length == 1) {
        $('body').removeClass('modal-open');
        $('body').removeAttr('style');
    }
    //$(".modal:visible:last").remove();
    $("#"+id).hide();
    $("#"+id).remove();
   // $("#" + id).display = "none";
    $('.modal-backdrop').remove();
    
    
   // $('body').removeAttr('class');
   // $('body').removeAttr('style');
   // alert('id' + id);

}
