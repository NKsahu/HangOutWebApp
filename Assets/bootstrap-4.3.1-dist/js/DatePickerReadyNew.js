$(document).on("focusin", ".timepiker", function () {
    $(this).timepicker({ showMeridian: false });

});

$(document).on("focusin", ".timepiker24", function () {
    $(this).timepicker({
        timeFormat: 'HH:mm:ss',
        showMeridian: false
    });
});
//$(document).on("focusin", ".datepicker", function () {
//    $(this).datepicker({
//        format: "dd/mm/yyyy",
//        todayHighlight: true,
//        "autoclose": true
//    });
//})
$(document).on("focusin", ".datefield,.DateField", function () {
    //$(this).datepicker();
    //alert("hi");
//$(this).datepicker();
//var d= $(this).val().split("/");
//M.Datepicker.getInstance($(this)).setDate(new Date(parseInt(d[2]),parseInt(d[1])-1,parseInt(d[0])), true);
   // console.log("AAYAdate");
    $(this).datepicker({
        format: "dd-mm-yyyy",
        todayHighlight: true,
        "autoclose": true
    });
    //$(this).datepicker("option", "dateFormat", "dd-mm-yy");
});
$(document).on("focusin", ".datetimefield", function () {
    //$(this).datepicker();
    //alert("hi");
    $(this).datetimepicker({
        //changeMonth: true,
        //changeYear: true,
        //showTime: true,
        format: "dd/mm/yyyy HH:ii:ss P",
        todayHighlight: true,
        "autoclose": true
    });
    //$(this).datepicker("option", "dateFormat", "dd-mm-yy");
});

$(document).on("focusin", ".datefield-week", function () {
    $(this).datepicker({
        format: "DD dd/mm/yyyy",
        maxViewMode: 0,
        todayBtn: true,
        keyboardNavigation: false,
        daysOfWeekDisabled: "1,2,3,4,5,6",
        daysOfWeekHighlighted: "0",
        calendarWeeks: true,
        autoclose: true
    });
});
$(document).on("focusin", ".datefield-week1", function () {
    $(this).datepicker({
        format: "DD dd/mm/yyyy",
        maxViewMode: 0,
        todayBtn: true,
        keyboardNavigation: false,
        daysOfWeekDisabled: "0,1,2,3,4,6",
        daysOfWeekHighlighted: "5",
        calendarWeeks: true,
        autoclose: true
    });
});

$(document).on("focusin", ".datefield-month", function () {
    $(this).datepicker({
        format: "MM yyyy",
        minViewMode: 1,
        maxViewMode: 2,
        todayBtn: true,
        keyboardNavigation: false,
        calendarWeeks: true,
        autoclose: true
    });
});

$(document).on("focusin", ".datefield-year", function () {
    $(this).datepicker({
        format: "yyyy",
        minViewMode: 1,
        maxViewMode: 2,
        todayBtn: true,
        keyboardNavigation: false,
        calendarWeeks: true,
        autoclose: true
    });
});

$(document).on("focusin", ".datefield2", function () {
    //$(this).datepicker();
    $(this).datepicker({
        changeMonth: true,
        changeYear: true,
        showTime: true, dateFormat: "dd/mm/yy",
        minDate: new Date(2015, 2 - 1, 10),
    });
    //$(this).datetimepicker({
    //    language: 'en',
    //    pick12HourFormat: true
    //});
    //$(this).datepicker("option", "dateFormat", "dd-mm-yy");
});
$(document).on("focusin", ".datefield1", function () {
    //$(".datefield1").live("focusin", function () {
    //$(this).datepicker();
    $(this).datetimepicker({
        format: "dd/mm/yyyy HH:ii:ss P",
        showMeridian: true,
        autoclose: true,
        todayBtn: true,
        todayHighlight: true,
        
       // default: true
       // endDate: '+0d'
        //changeMonth: true,
        //changeYear: true, 
        //showTime: true,
        //dateFormat: "dd/mm/yy",
        //timeFormat:"hh:mm tt",
        //showOn: "button",
        //buttonImage: "image/calendar.gif",
        //buttonImageOnly: true,
        //stepMinute: 1
    });
    //$(this).datepicker("option", "dateFormat", "dd-mm-yy");

});
//}
////$(function () {
////    $(".datefield").datepicker({
////        changeMonth: true,
////        changeYear: true
////    });
////});
////$("#master").delegate(".datefield", "click", function () {
////    $(this).datepicker({
////        changeMonth: true,
////        changeYear: true
////    });
////});



$(document).on("focusin", ".bootstrap-datepicker", function () {
    //$(this).datepicker();
    alert("hi");
    $(this).datepicker({
        //changeMonth: true,
        //changeYear: true,
        todayHighlight: true,
        dateFormat: 'dd/mm/yy', autoclose: true,
    });


    //    .on('changeDate', function (ev) {
    //    $(this).bsdatepicker('hide');
    //});
});
$(document).on("focusin", ".bootstrap-datepicker2", function () {
    //$(this).datepicker();
    $(this).datepicker({
        changeMonth: true,
        changeYear: true,
        showTime: true, dateFormat: "dd/mm/yy",
        minDate: new Date(2015, 2 - 1, 10),
    });
    //$(this).datetimepicker({
    //    language: 'en',
    //    pick12HourFormat: true
    //});
    //$(this).datepicker("option", "dateFormat", "dd-mm-yy");
});
//$(document).on("focusin", ".datefield1", function () {
//    //$(".datefield1").live("focusin", function () {
//    //$(this).datepicker();
//    $(this).datetimepicker({
//        format: "dd/mm/yyyy HH:ii P",
//        showMeridian: true,
//        autoclose: true,
//        todayBtn: true
//        //changeMonth: true,
//        //changeYear: true, 
//        //showTime: true,
//        //dateFormat: "dd/mm/yy",
//        //timeFormat:"hh:mm tt",
//        //showOn: "button",
//        //buttonImage: "image/calendar.gif",
//        //buttonImageOnly: true,
//        //stepMinute: 1
//    });
//    //$(this).datepicker("option", "dateFormat", "dd-mm-yy");
//});
//}