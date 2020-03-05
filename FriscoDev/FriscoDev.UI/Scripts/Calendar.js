

function encodeCalendar(cval, d) {
    var sepCalendar = getStorageItem("sepcalendar");
    if (cval > 0) {
        //marker color
        if (sepCalendar.indexOf(d) <= -1) {
            sepCalendar = sepCalendar + "," + d;
        }
    }
    else {
        //remove select
        if (sepCalendar.indexOf(d) > -1) {
            sepCalendar = sepCalendar.replace(d, "");
        }

    }
    sepCalendar = sepCalendar.replace(",,", ",");
    setStorageItem("sepcalendar", sepCalendar);
}

function decodeCalendar(d) {
    var isExist = false;
    //setStorageItem("sepcalendar", "");
    var sepCalendar = getStorageItem("sepcalendar");
    if (sepCalendar != "" && sepCalendar.indexOf(d) > -1) {
        isExist = true;
    }
    return isExist;
}

var cal_months = new Array("JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEPT", "OCT", "NOV", "DEC");
var cal_daysInMonth = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
var cal_days = new Array("Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat");

var newCal;
var curIndex = 0;

function getCalendarToday(ci) {
    this.nowDate = new Date();
    this.nowDate = new Date(this.nowDate.getFullYear(), this.nowDate.getMonth() + ci, this.nowDate.getDate());
    this.year = this.nowDate.getFullYear();
    this.month = this.nowDate.getMonth();
    this.day = this.nowDate.getDate();
}

function getCalendarDays(month, year) {
    if (1 == month) {
        return ((0 == year % 4) && (0 != (year % 100))) || (0 == year % 400) ? 29 : 28;
    }
    else {
        return cal_daysInMonth[month];
    }
}

//Create calendar object
function CreateCalendar() {

    try {
        curIndex = parseInt($("#curIndex").val());
        $("#newCalendarDiv").html("");
        for (var mc = 0; mc < 12; mc++) {
            var today = new getCalendarToday(mc + curIndex);
            var year = today.year;
            var month = today.month;
            newCal = new Date(year, month, 1)
            var day = -1;
            var startDay = newCal.getDay();
            var endDay = getCalendarDays(newCal.getMonth(), newCal.getFullYear());
            var daily = 0;
            if ((today.year == newCal.getFullYear()) && (today.month == newCal.getMonth())) {
                day = today.day;
            }
            //class="CalendarOld"
            var sourceDiv = $("#sourceCalendarDiv").html();
            sourceDiv = sourceDiv.replace("class=\"CalendarOld\"", "class=\"Calendar\"");
            sourceDiv = sourceDiv.replace("id=\"calTable\"", "id=\"calTable_" + mc + "\"");
            sourceDiv = sourceDiv.replace("id=\"YearMonth\"", "id=\"YearMonth_" + mc + "\"");
            sourceDiv = sourceDiv.replace("id=\"calCell\"", "id=\"calCell_" + mc + "\"");
            sourceDiv = sourceDiv.replace("id=\"cell_", "id=\"cell_" + mc + "");
            $("#newCalendarDiv").html($("#newCalendarDiv").html() + sourceDiv);

            $("#textarea").val($("#newCalendarDiv").html());

            var caltable = $("#calTable_" + mc + " tbody");
            var calTableRows = $(caltable).children("tr");

            var intDaysInMonth = getCalendarDays(newCal.getMonth(), newCal.getFullYear());
            for (var intWeek = 0; intWeek < calTableRows.length; intWeek++) {
                for (var intDay = 0; intDay < calTableRows[intWeek].cells.length; intDay++) {
                    var cell = calTableRows[intWeek].cells[intDay];
                    var montemp = (newCal.getMonth() + 1) < 10 ? ("0" + (newCal.getMonth() + 1)) : (newCal.getMonth() + 1);
                    if ((intDay == startDay) && (0 == daily)) {
                        daily = 1;
                    }
                    var daytemp = daily < 10 ? ("0" + daily) : (daily);
                    var d = "<" + newCal.getFullYear() + "-" + montemp + "-" + daytemp + ">";
                    var newd = newCal.getFullYear() + "-" + montemp + "-" + daytemp;
                    cell.style.background = "#ffffff";
                    if (day == daily && (mc + curIndex) == 0) {
                        if (intDay == 6 || intDay == 0) {
                            cell.className = "DayNowEnd" + " " + newd;
                        }
                        else {
                            cell.className = "DayNow" + " " + newd;
                        }
                    }
                    else if (intDay == 6) {
                        cell.className = "DaySat" + " " + newd;
                    }
                    else if (intDay == 0) {
                        cell.className = "DaySun" + " " + newd;
                    }
                    else {
                        cell.className = "Day" + " " + newd;
                    }

                    if ((daily > 0) && (daily <= intDaysInMonth)) {
                        cell.innerText = daily;
                        var isExist = decodeCalendar(newd);
                        if (isExist) {
                            cell.style.background = "#7f70ff";
                        }
                        daily++;
                    } else {
                        cell.className = "CalendarTD";
                        cell.innerText = "";
                    }
                }
            }

            $("#YearMonth_" + mc).html(cal_months[month] + " " + year);
        }

        var bl = $("#ckb_IncWeedends").is(":checked");
        var caltabletd = $(".Calendar tbody td");

        //no checked
        $.each(caltabletd, function () {
            var cur_font_color = $(this).css("color");
            if (cur_font_color == "rgb(235, 165, 171)") {
                if (bl) {
                    var cur_bg_color = $(this).css("background-color");
                    if (cur_bg_color != "rgb(127, 112, 255)") {
                        $(this).css("background-color", "#ffffff");
                    }
                    $(this).attr("onclick", "calendartdClick(this)");
                }
                else {
                    //$(this).css("background-color", "#ebe9ff");
                    $(this).css("background-color", "#E2E3E3");
                    $(this).removeAttr("onclick");
                }
            }

        });

    }
    catch (e) { alert(e);}

}

//change month
function changeMonth(ind) {
    curIndex = parseInt($("#curIndex").val());
    if (ind > 0) {
        //add
        $("#curIndex").val(curIndex + 1);
    }
    else {
        //sub
        $("#curIndex").val(curIndex - 1);
    }
    CreateCalendar();
}

//Cur calendar td click
function calendartdClick(obj) {
    if ($(obj).text() != "") {
        var curDate = $(obj).attr("class");
        curDate = curDate.split(" ")[1];
        var cur_bg_color = $(obj).css("background-color");
        //alert(cur_bg_color);
        if (cur_bg_color == "rgb(255, 255, 255)") {
            $(obj).css("background-color", "#7f70ff");
            encodeCalendar(1, curDate)
        }
        else {
            $(obj).css("background-color", "#ffffff");
            encodeCalendar(-1, curDate)
        }
    }
}