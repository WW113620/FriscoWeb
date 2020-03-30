
function ConvertSize(size) {
    size = parseInt(size)
    if (size == 3)
        return 18;
    else if (size == 2)
        return 15;
    else
        return 12;
}

function getScheduleList(name) {
    var html = "";
    if (!name)
        name = "";
    $ajaxFuncNoLoading("/PMG/GetScheduledOperationList", {}, function (res) {
        if (res && res.length > 0) {
            for (var i = 0; i < res.length; i++) {
                if (i % 2 == 0) {
                    html += '<tr>';
                } else {
                    html += '<tr class="widget-tr-backgroud">';
                }
                if (name == res[i].Name) {
                    html += ' <td class="text-center"><input type="radio" name="scheduled" value="' + res[i].Name + '" data-pmgid="' + res[i].PMGID + '" checked="checked" data-size="' + res[i].DisplayType + '" onclick="checkedOne(this)"/></td>';
                } else {
                    html += ' <td class="text-center"><input type="radio" name="scheduled" value="' + res[i].Name + '" data-pmgid="' + res[i].PMGID + '" data-size="' + res[i].DisplayType + '" onclick="checkedOne(this)"/></td>';
                }
                
                html += ' <td class="text-center">' + res[i].OperationName + '</td>'+
                ' <td class="text-center">' + res[i].DatePeriod + '</td>' +
                ' <td class="text-center">' + res[i].TimePeriod + '</td>' +
                ' <td class="text-center">' + res[i].Recurrence + '</td>' +
                ' <td class="text-center">' + res[i].Description + '</td>';
                html += '</tr>';
            }
        } else {
            html += '<tr><td class="text-center" colspan="5">No scheduled list</td></tr>';
        }

        $("#dataBind").html(html);
    });
}

function checkedOne(self) {
    var name = $(self).val();
    var pmgId = $(self).attr("data-pmgid");
    var size = $(self).attr("data-size");
    $ajaxFunc("/PMG/GetScheduledByOperationName", { "operationName": name, "displaySize": size, "PMGID": pmgId }, function (res) {
        console.log(res)
        if (res.code == 0) {
            var item = res.model;
            $("#operationName").val(item.operationName)
            $("#PMGID").val(item.PMGID)

            $("#idleDisplayMode").val(item.idleDisplayMode)

            getBindPage("idleDisplayPage", item.idleDisplayPage, item.IdlePageList);
            getBindPage("limitDisplayPage", item.limitDisplayPage, item.LimitPageList);
            getBindPage("alertDisplayPage", item.alertDisplayPage, item.AlertPageList);
            
            $("#limitSpeed").val(item.limitSpeed)
            $("#limitDisplayMode").val(item.limitDisplayMode)

            $("#limitActionType").val(item.limitActionType)

            $("#alertSpeed").val(item.alertSpeed)
            $("#alertDisplayMode").val(item.alertDisplayMode)

            $("#alertActionType").val(item.alertActionType)

            if (item.dateRangeType == 0) {
                $("#Calendar").prop('checked', false).parent().removeClass("checked")
                $("#DateRange").prop('checked', true).parent().addClass("checked")
                $("#calendarFilename").html('')
            } else {
                $("#DateRange").prop('checked', false).parent().removeClass("checked")
                $("#Calendar").prop('checked', true).parent().addClass("checked")
            }

            $("#StartDate").val(item.strStartDate)
            $("#StopDate").val(item.strStopDate)

            var selectedDays = item.selectedDays
            if (selectedDays == "Daily") {
                $("#days").prop('checked', false).parent().removeClass("checked")
                $("#daily").prop('checked', true).parent().addClass("checked")
                $("input[name='week']").each(function () {
                    $(this).prop('checked', false).parent().removeClass("checked")
                })
            } else {
                $("#daily").prop('checked', false).parent().removeClass("checked")
                $("#days").prop('checked', true).parent().addClass("checked")
                var selectedArray = selectedDays.split(',');
                $("input[name='week']").each(function () {
                    $(this).removeAttr("disabled");
                    $(this).prop('checked', false)
                    if ($.inArray($(this).val(), selectedArray) >= 0) {
                        $(this).prop('checked', true).parent().addClass("checked")
                    }
                })
            }

            $("#StartTime").val(item.strStartTime)
            $("#StopTime").val(item.strStopTime)

        } else {

        }
    });
}

$("#Calendar").click(function () {
    changeDate(1)
})

$("#DateRange").click(function () {
    changeDate(0)
})

$("#daily").click(function () {

    $("input[name='week']").each(function () {
        $(this).prop('checked', false).parent().removeClass("checked")
        $(this).attr("disabled", true);
    })

})
$("#days").click(function () {
    $("input[name='week']").each(function () {
        $(this).removeAttr("disabled");
    })
})


function changeDate(type) {
    if (type == 1) {
        $("#StartDate,#StopDate").attr("disabled", true);
        $("#days,#daily").attr("disabled", true);
        $("input[name='week']").each(function () {
            $(this).attr("disabled", true);
        })
    } else {
        $("#StartDate,#StopDate").removeAttr("disabled");
        $("#days,#daily").removeAttr("disabled");
        $("input[name='week']").each(function () {
            $(this).removeAttr("disabled");
        })
    }
}

function getBindPage(id, page, list) {
    var selectPage = "";
    if (page && page.name)
        selectPage = page.name;

    var _html = "";

    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            var value = $.trim(item.value);
            if (item.Text == selectPage) {
                _html += '<option value="' + value + '" selected="selected">' + item.Text + '</option>';
            } else {
                _html += '<option value="' + value + '">' + item.Text + '</option>';
            }
        }
    }
    $("#" + id).html(_html)
}



function Delete() {
    var schedule = $("#dataBind").find("input[name='scheduled']:checked").val()
    if (!schedule) {
        LayerMsg("Please select a scheduled operation")
        return false;
    }
    var pmgid = $("#dataBind").find("input[name='scheduled']:checked").attr("data-pmgid");
    var size = $("#dataBind").find("input[name='scheduled']:checked").attr("data-size");
    $ajaxFunc("/PMG/DeleteScheduledOperation", { "operationName": schedule, "displaySize": size, "PMGID": pmgid }, function (res) {
        if (res.code == 0) {
            LayerMsg("Delete successfully!")
            window.location.reload();
        } else {
            LayerMsg(res.msg)
        }
    });
}

function Add() {
    var schedule = $("#dataBind").find("input[name='scheduled']:checked").val()
    $("#dataBind").find("input[name='scheduled']:checked").prop('checked', false);
    var name = $.trim($("#operationName").val());
    if (!name) {
        LayerMsg("Operation name is empty!")
        return false;
    }
    if (name.length > 27) {
        LayerMsg("Operation name cannot exceeds 27 characters!")
        return false;
    }
    var size = ConvertSize($("#PMGID").find("option:selected").attr("data-size"));
    if (!size)
        size = 12;
    var params = getParameters("", size);
    $ajaxFunc("/PMG/AddScheduledOperation", params, function (res) {
        if (res.code == 0) {
            LayerMsg("Add successfully!")
            getScheduleList(res.msg);
        } else {
            LayerMsg(res.msg)
        }
    });
}

function Edit() {
    var schedule = $("#dataBind").find("input[name='scheduled']:checked").val()
    if (!schedule) {
        LayerMsg("Please select a scheduled operation")
        return false;
    }

    var name = $.trim($("#operationName").val());
    if (!name) {
        LayerMsg("Operation name is empty!")
        return false;
    }
    if (name.length > 27) {
        LayerMsg("Operation name cannot exceeds 27 characters!")
        return false;
    }

    var pmgid = $("#dataBind").find("input[name='scheduled']:checked").attr("data-pmgid");
    var size = $("#dataBind").find("input[name='scheduled']:checked").attr("data-size");

    var params = getParameters(schedule, size);

    $ajaxFunc("/PMG/SaveScheduledOperation", params, function (res) {
        if (res.code == 0) {
            LayerMsg("Save successfully!")
            getScheduleList(schedule);
        } else {
            LayerMsg(res.msg)
        }
    });
}

function getParameters(name, displayType) {

    var calendarFilename = "", strStartDate = "", strStopDate = "", selectedDays = "";
    var days = $("input[name='days']:checked").val();
    if (!days)
        days = "daily";
    var dateRangeType = $("input[name='Calendar']:checked").val();
    if (!dateRangeType || dateRangeType == "0") {
        dateRangeType = 0;
        strStartDate = $("#StartDate").val();
        strStopDate = $("#StopDate").val();
        if (days == "days") {
            selectedDays = $("input[name='week']:checkbox:checked").map(function () { return $(this).val() }).get().join(",");
        }

    } else {
        dateRangeType = 1;//Calendar
        calendarFilename = $("#calendarFilename").val();
        if (!calendarFilename) {
            calendarFilename = "";
        }
    }

    var idleDisplayPageName = getEmptyValue($("#idleDisplayPage").val());

    var limitDisplayPageName = getEmptyValue($("#limitDisplayPage").val());

    var alertDisplayPageName = getEmptyValue($("#alertDisplayPage").val());

    var params = {
        "name": name,
        "operationName": $.trim($("#operationName").val()),
        "PMGID": $("#PMGID").val(),
        "displayType": displayType,
        "idleDisplayMode": $("#idleDisplayMode").val(),

        "idleDisplayPageName": idleDisplayPageName,

        "limitSpeed": $("#limitSpeed").val(),
        "limitDisplayMode": $("#limitDisplayMode").val(),

        "limitDisplayPageName": limitDisplayPageName,

        "limitActionType": $("#limitActionType").val(),
        "alertSpeed": $("#alertSpeed").val(),
        "alertDisplayMode": $("#alertDisplayMode").val(),

        "alertDisplayPageName": alertDisplayPageName,

        "alertActionType": $("#alertActionType").val(),
        "dateRangeType": dateRangeType,
        "calendarFilename": calendarFilename,
        "StartDate": strStartDate,
        "StopDate": strStopDate,
        "selectedDays": selectedDays,
        "StartTime": $("#StartTime").val(),
        "StopTime": $("#StopTime").val()
    }

    return params;
}

function getEmptyValue(val) {
    if (!val)
        return "";

    return val;

}


$("#idleDisplayMode").change(function () {
    var actionType = $(this).val();
    var size = ConvertSize($(this).attr("data-size"));
    SelectChangeDisplay("idleDisplayPage", actionType, size)
});

$("#limitDisplayMode").change(function () {
    var actionType = $(this).val();
    var size = ConvertSize($(this).attr("data-size"));
    SelectChangeDisplay("limitDisplayPage", actionType, size)
});

$("#alertDisplayMode").change(function () {
    var actionType = $(this).val();
    var size = ConvertSize($(this).attr("data-size"));
    SelectChangeDisplay("alertDisplayPage", actionType, size)
});


function SelectChangeDisplay(id, actionType, size) {
    if (actionType == 0 || actionType == 1 || actionType == 5 || actionType == 6) {
        $("#" + id).html("")
        return;
    }

    $.ajax({
        type: "POST",
        url: "/PMG/GetPageDisplay",
        data: { "pmgInch": size, "actionType": parseInt(actionType) },
        timeout: 3000,
        success: function (res) {
            if (res && res.length > 0) {
                var _html = "";
                for (var i = 0; i < res.length; i++) {
                    var item = res[i];
                    _html += '<option value="' + StrTrim(item.value) + '">' + item.Text + '</option>';
                }
                $("#" + id).html(_html)
            } else {
                $("#" + id).html("")
            }
        }

    })

}



$(function () {
    getScheduleList("")

    $("input[name='week']").each(function () {
        $(this).attr("disabled", true);
    })
})