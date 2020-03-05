function selected(id, val) {
    var obj = document.getElementById(id);
    for (i = 0; i < obj.length; i++) {
        if (obj.options[i].value == val) {
            obj.options[i].selected = true;
        }
    }
}


function getStorageItem(key) {
    var itemtemp = window.localStorage.getItem(key);
    var rettemp = "";
    if (itemtemp != null && itemtemp != "" && itemtemp != "undefined" && itemtemp != "null") {
        rettemp = itemtemp;
    }
    return rettemp;
}

function setStorageItem(key, value) {
    window.localStorage.setItem(key, value);
}
function removeStorageItem(key) {
    window.localStorage.removeItem(key);
}
function getNowFormatDate(dates, datepart) {

    var CurrentDate = "";
    try {
        var _dates = new Date();
        if (dates != "") {
            dates = dates.replace("T", " ");
            dates = dates.replace(/-/g, "/");
            dates = dates.substring(0, 19);
            _dates = new Date(dates);
        }

        var mon = ["JAN.", "FEB.", "MAR.", "APR.", "MAY.", "JUN.", "JUL.", "AUG.", "SEPT.", "OCT.", "NOV.", "DEC."];

        var Year = 0; //
        var Month = 0; //
        var Day = 0; //
        var Hours = 0; //
        var Minute = 0; //
        var Seconds = 0; //

        var _Month = ""; //
        var _Day = ""; //
        var _Hours = ""; //
        var _Minute = ""; //
        var _Seconds = ""; //

        Year = _dates.getFullYear();
        Month = _dates.getMonth() + 1;
        Day = _dates.getDate();
        Hours = _dates.getHours();
        Minute = _dates.getMinutes();
        Seconds = _dates.getSeconds();

        if (Month >= 10) {
            _Month = Month;
        }
        else {
            _Month = "0" + Month;
        }

        if (Day >= 10) {
            _Day = Day;
        }
        else {
            _Day = "0" + Day;
        }

        if (Hours >= 10) {
            _Hours = Hours;
        }
        else {
            _Hours = "0" + Hours;
        }

        if (Minute >= 10) {
            _Minute = Minute;
        }
        else {
            _Minute = "0" + Minute;
        }

        if (Seconds >= 10) {
            _Seconds = Seconds;
        }
        else {
            _Seconds = "0" + Seconds;
        }
        //alert(datepart);

        switch (datepart) {
            case "yyyy":
                CurrentDate = Year + "";
                break;
            case "yyyyMM":
                CurrentDate = Year + "-" + _Month;
                break;
            case "yyyy-MM-dd":
                CurrentDate = Year + "-" + _Month + "-" + _Day;
                break;
            case "yyyyMMdd":
                CurrentDate = Year + "年" + _Month + "月" + _Day + "日";
                break;
            case "yyyy.MM.dd":
                CurrentDate = Year + "." + _Month + "." + _Day;
                break;
            case "MMddyyyy":
                CurrentDate = mon[_Month - 1] + _Day + "." + Year;
                break;
            case "yyyyMMdd HH:mm":
                CurrentDate = Year + "." + _Month + "." + _Day + " " + _Hours + ":" + _Minute;
                break;
            case "yyyy-MM-dd HH:mm":
                CurrentDate = Year + "-" + _Month + "-" + _Day + " " + _Hours + ":" + _Minute;
                break;
            case "HH:mm":
                CurrentDate = _Hours + ":" + _Minute;
                break;
            case "MM-dd HH:mm":
                CurrentDate = _Month + "-" + _Day + " " + _Hours + ":" + _Minute;
                break;
            default:
                CurrentDate = _dates;
                break;
        }
    }
    catch (e) {
        alert(e);
        CurrentDate = dates;
    }
    return CurrentDate;
}

Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //
        "d+": this.getDate(), //
        "h+": this.getHours(), //
        "m+": this.getMinutes(), //
        "s+": this.getSeconds(), //
        "q+": Math.floor((this.getMonth() + 3) / 3), //
        "S": this.getMilliseconds() //
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}