var layerIndex = null;

function layerMes(Msg, Dom) {
    layerIndex = layer.tips(Msg, Dom, {
        tips: [3, '#78BA32']
    });
}

function CuteStr(str, len)
{
    if (!str)
        return str;
    if (str.length > len) {
        return str.substr(0,len)+'...';
    } else {
        return str;
    }

}

function layerClose() {
    if (layerIndex)
        layer.close(layerIndex);
}

function LayerAlert(Msg) {
    layerIndex = layer.alert(Msg, { title: false, closeBtn: 0, offset: ['35%', '45%'], btn: ['OK'] });
}


function LayerMsg(Msg) {
    layerIndex=layer.msg(Msg, { time: 1200, offset: ['35%', '44%'] });
}

function GetTextareaContect(id) {
    var _content = $("textarea#" + id).val();
    if (!_content) { _content = $("textarea#" + id).html(); }
    if (!_content) { _content = $("textarea#" + id).text(); }
    return $.trim(_content);
}

function GetTextareaByDom(dom) {
    var _content = dom.val();
    if (!_content) { _content = dom.html(); }
    if (!_content) { _content = dom.text(); }
    return $.trim(_content);
}


function CheckMail(mail) {
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (filter.test(mail)) {
        return true;
    }
    else {
        return false;
    }
}


function StrTrim(str) {
    if (!str)
        return "";
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}


function isPositiveInteger(s) {
    var re = /^[0-9]+$/;
    return re.test(s)
}
function isEmail(str) {
    var reg = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/;
    return reg.test(str);
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

String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{\{|\}\}|\{(\d+)\}/g, function (m, n) {
        if (m == "{{") { return "{"; }
        if (m == "}}") { return "}"; }
        return args[n];
    });
};
String.prototype.cutStr = function (maxLength) {
    var args = arguments;
    if (args == undefined || this == undefined || this == "") {
        return "";
    }
    if (this.length > args[0]) {
        return this.substr(0, maxLength - 1) + "...";
    }
    else {
        return this;
    }
};
String.prototype.formatTime = function () {
    if (this == undefined || this == "") {
        return "1900-01-01 00:00:00";
    }
    return this.split("T")[0] + " " + this.split("T")[1].split("+")[0];
}

function IsURL(str_url) {
    var strRegex = '[a-zA-z]+://[^\s]*';
    var reg = new RegExp(strRegex);
    return reg.test(str_url);
}

function IsNum(num) {
    var strRegex = /^(([1-9][0-9]*)|(([0]\.\d{1,2}|[1-9][0-9]*\.\d{1,2})))$/;
    var reg = new RegExp(strRegex);
    return reg.test(num);
}



function isEmail(str) {
    var reg = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((.[a-zA-Z0-9_-]{2,3}){1,2})$/;
    return reg.test(str);
}

function selected(id, val) {
    var obj = document.getElementById(id);
    for (i = 0; i < obj.length; i++) {
        if (obj.options[i].value == val) {
            obj.options[i].selected = true;
        }
    }
}

function isExistValue(text, domainUrl) {
    var flag = false;
    $.ajax({
        url: domainUrl,
        data: { text: text },
        dataType: 'json', type: "POST", async: false,
        success: function (json) {
            if (json.status == "y") {
                flag = true;
            } else {
                flag = false;
            }
        }
    });
    return flag;
}


function isExistinputName(value, domainUrl) {
    var flag = false;
    $.ajax({
        url: domainUrl,
        data: "{'value':'" + value + "'}",
        type: "POST", dataType: 'json', async: false,
        contentType: "Application/json;charset=utf-8",
        success: function (result) {
            var jsonObj = JSON.parse(result.d);
            if (jsonObj.errorCode == 200) {
                flag = true;
            } else {
                flag = false;
            }
        }
    });
    return flag;
}


function convertStrToDate(str) {
    if (str == undefined || str.length <= 0) {
        return null;
    }
    else {
        var arr = str.match(/\d+/g);
        if (arr != null && arr.length >= 3) {
            var date = new Date(arr[0], arr[1], arr[2]);
            switch (arr.length) {
                case 4:
                    date.setHours(arr[3]);
                    break;
                case 5:
                    date.setHours(arr[3]);
                    date.setMinutes(arr[4]);
                    break;
                case 6:
                    date.setHours(arr[3]);
                    date.setMinutes(arr[4]);
                    date.setSeconds(arr[5]);
                    break;
            }
            return date;
        }
        else
            return null;
    }
}

function formatDate(date) {
    if (date === "" || date === null)
        return "";
    var d = new Date(date),
      month = '' + (d.getMonth() + 1),
      day = '' + d.getDate(),
      year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
}


function CountWords(textarea, number, total1, total2) {
    var counter = textarea.val().length; 
    number.text(Math.abs(total1 - counter));
    textarea.keyup(function () {
        var text = textarea.val();
        var counter = text.length;
        number.text(Math.abs(total2 - counter));
    })   
}

function setCheckBoxCheckedValue(el, values, splitStr) {
    var mycheck = document.getElementsByName(el);
    for (var j = 0; j < mycheck.length; j++) {
        mycheck[j].checked = false;
    }
    if (values == undefined || values == "") {
        return;
    }
    var valuesArr = values.split(splitStr);
    var len = valuesArr.length;
    var value = "";
    for (var i = 0; i < len; i++) {
        value = valuesArr[i];
        value = value.replace(splitStr, "")
        for (var j = 0; j < mycheck.length; j++) {
            if (mycheck[j].value == value) {
                mycheck[j].checked = true;
                break;
            }
        }
    }
}


function setCheckedValue(el, checkedValue) {
    var myradio = document.getElementsByName(el);
    for (var i = 0; i < myradio.length; i++) {
        if (myradio[i].value == checkedValue) {
            myradio[i].checked = true;
            break;
        }
    }
}

