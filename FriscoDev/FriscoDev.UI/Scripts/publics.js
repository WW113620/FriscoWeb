
function StrTrim(str) {
    if (str == "")
        return "";
    return str.replace(/(^\s*)|(\s*$)/g, "");
}
function isEmail(str) {
    var reg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
    return reg.test(str);
}

function CheckMail(mail) {
    var filter = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
    if (filter.test(mail)) {
        return true;
    }
    else {
        return false;
    }
}

function isExistValue(value, domainUrl) {
    var flag = false;
    $.ajax({
        url: domainUrl,
        data: { value: value },
        dataType: 'json', type: "POST", async: false,
        success: function (result) {
            if (result.errorCode == 200) {
                flag = true;
            } else {
                flag = false;
            }
        }
    });
    return flag;
}

function SetSession(strPmd) {
    $.getJSON("/Home/SetPmdSession?strPmd=" + strPmd, null, function (data) { });
    initSelected();
}
function initSelected() {
    var imis = $("#PId").val();
    SetUpMenuHide(imis);
    $(".left-nav-products-list").find("li").each(function () {
        var allImis = $(this).attr("data-pmdId");
        var arrIMSI = allImis.split('_');
        $("#left-nav-btns").find("li").removeClass("active");
        $("#left-nav-btns").find("#" + arrIMSI[0] + "_device").addClass("active");
        if (imis == arrIMSI[1]) {
            $(this).parent("ul").show().siblings("ul").hide();
            $(this).find("span").addClass("redColor");
            return false;
        }
    })
}

function SetUpMenuHide(imsi) {
    $.ajax({
        url: "/Home/GetDeviceType",
        data: { imsi: imsi },
        dataType: 'json', type: "POST",
        success: function (result) {
            if (result.errorCode == 200) {
                DefaultOnSetUp(result.deviceType);
            }
        }
    });
}
function DefaultOnSetUp(type) {
    if (type == 2) {
        $("li.TCDCalendarAndSetup").hide();
    }
    else {
        $("li.TCDCalendarAndSetup").show();
    }
}
//Com menu
var defaultLeftNav = [{
    text: 'ALL',
    href: '',
    nodes: []
}];

//admin menu
var ltLeftNav = [{
    text: 'Menu',
    href: '',
    nodes: [{
        text: 'User',
        href: '/User/Index'
    }, {
        text: 'Device',
        href: '/Device/Index'
    }]
}];

function inputError(_this, _temp) {
    if (_temp) {
        _this.parents(".form-group").removeClass("has-error");
    } else {
        _this.parents(".form-group").addClass("has-error");
    }
}

//
function init(tree) {
    $('#left-tree-nav').treeview({
        expandIcon: "fa fa-fw fa-folder-o",
        collapseIcon: "fa fa-fw fa-folder-open",
        //color: "#222222",
        //backColor: "white",
        //selectedColor: "white",
        selectedBackColor: "#c4c4c4",// "#8c7d7d",
        showBorder: false,
        levels: 3,
        highlightSelected: true,
        enableLinks: true,
        data: tree,
        //selectedIcon: "glyphicon glyphicon-ok",
        //uncheckedIcon: "glyphicon glyphicon-remove"
    });
}



function LoadMenu() {
    $.getJSON("/Home/Devices", null, function (data) {
        if (data.Success) {
            //init(data.Devices);
            $("#left-tree-nav ul li").each(function (index) {
                //$(this).css("color", "inherit");
                //$(this).css("background-color", "#fff");
                var curhref = $(this).find("a").attr("href");
                if (curhref != undefined) {
                    var _curpmdid = "'" + $("#PmdId").val() + "'";
                    if (_curpmdid != "0") {
                        var _indexval = curhref.indexOf(_curpmdid);
                        if (_indexval > -1) {
                            $(this).css("color", "white");
                            $(this).css("background-color", "#c4c4c4");// 8c7d7d
                        }
                    }
                }
            });

        } else {
            if (!data.Logged) {
                location.href = "/Login/Index";
            }
        }
    });
    var _xval = $("#XValue").val();
    var _yval = $("#YValue").val();
    var _did = $("#PId").val();
    var _pid = $("#PmdId").val();
    initialize(_xval, _yval, _did, _pid);
}

//从字符串中提取数字并将其转换为日期
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

