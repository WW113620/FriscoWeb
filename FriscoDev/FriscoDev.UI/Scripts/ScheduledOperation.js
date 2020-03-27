

function getScheduleList() {
    var html = "";
    $ajaxFunc("/PMG/GetScheduledOperationList", {}, function (res) {
        if (res && res.length > 0) {
            for (var i = 0; i < res.length; i++) {
                if (i % 2 == 0) {
                    html += '<tr>';
                } else {
                    html += '<tr class="widget-tr-backgroud">';
                }
                html += ' <td class="text-center"><input type="radio" name="scheduled" value="' + res[i].Name + '" data-pmgid="' + res[i].PMGID + '" onclick="checkedOne(this)"/></td>' +
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

function checkedOne(self)
{
    var name = $(self).val();
    var pmgId = $(self).attr("data-pmgid");
    

}

function ConvertSize(size) {
    size = parseInt(size)
    if (size == 1)
        return 12;
    else if (size == 2)
        return 15;
    else
        return 18;
}

$("#idleDisplayMode").change(function () {
    var actionType = $(this).val();
    var size = ConvertSize($(this).attr("data-size"));
    SelectChangeDisplay("idleDisplayPage", actionType,size)
});

$("#limitDisplayMode").change(function () {
    var actionType = $(this).val();
    var size = ConvertSize($(this).attr("data-size"));
    SelectChangeDisplay("limitDisplayPage", actionType,size)
});

$("#alertDisplayMode").change(function () {
    var actionType = $(this).val();
    var size = ConvertSize($(this).attr("data-size"));
    SelectChangeDisplay("alertDisplayPage", actionType,size)
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