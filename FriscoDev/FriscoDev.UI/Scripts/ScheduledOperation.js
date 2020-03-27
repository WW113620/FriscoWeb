
function ConvertSize(size) {
    size = parseInt(size)
    if (size == 1)
        return 12;
    else if (size == 2)
        return 15;
    else
        return 18;
}

function getScheduleList() {
    var html = "";
    $ajaxFuncNoLoading("/PMG/GetScheduledOperationList", {}, function (res) {
        if (res && res.length > 0) {
            for (var i = 0; i < res.length; i++) {
                if (i % 2 == 0) {
                    html += '<tr>';
                } else {
                    html += '<tr class="widget-tr-backgroud">';
                }
                html += ' <td class="text-center"><input type="radio" name="scheduled" value="' + res[i].Name + '" data-pmgid="' + res[i].PMGID + '" data-size="' + res[i].DisplayType + '" onclick="checkedOne(this)"/></td>' +
                        ' <td class="text-center">' + res[i].OperationName + '</td>' +
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

            $("#limitSpeed").val(item.limitSpeed)
            $("#limitDisplayMode").val(item.limitDisplayMode)

            $("#limitActionType").val(item.limitActionType)

            $("#alertSpeed").val(item.alertSpeed)
            $("#alertDisplayMode").val(item.alertDisplayMode)

            $("#alertActionType").val(item.alertActionType)


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

function Add() {
    var schedule = $("#dataBind").find("input[name='scheduled']:checked").val()
    $("#dataBind").find("input[name='scheduled']:checked").prop('checked', false);
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
            getScheduleList();
        } else {
            alert(res.msg)
        }
    });
}

function Edit() {
    var schedule = $("#dataBind").find("input[name='scheduled']:checked").val()
    if (!schedule) {
        LayerMsg("Please select a scheduled operation")
        return false;
    }
    var pmgid = $("#dataBind").find("input[name='scheduled']:checked").attr("data-pmgid");
    var size = $("#dataBind").find("input[name='scheduled']:checked").attr("data-size");
    $ajaxFunc("/PMG/DeleteScheduledOperation", { "operationName": schedule, "displaySize": size, "PMGID": pmgid }, function (res) {
        if (res.code == 0) {
            getScheduleList();
        } else {
            alert(res.msg)
        }
    });
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
    getScheduleList()
})