
$("#pmgInch").change(function () {
    resetDefault()
    getCompositeList();
})


var pageType = 4;

function getCompositeList() {
    var selectedValue = $("#hidPageName").val()
    if (!selectedValue)
        selectedValue = "";
    $.ajax({
        type: "POST",
        url: "/PMG/GetPageList",
        data: { "displaySize": parseInt($("#pmgInch").val()), "pageType": pageType },
        success: function (res) {
            if (res && res.length > 0) {
                var _html = "";
                for (var i = 0; i < res.length; i++) {
                    var item = res[i];
                    if (selectedValue == item.Text) {
                        _html += '<option value="' + item.value + '" selected="selected">' + item.Text + '</option>';
                    } else {
                        _html += '<option value="' + item.value + '">' + item.Text + '</option>';
                    }

                }
                $("#pageList").html(_html)
                if (selectedValue) {
                    getPageInfo()
                }
            } else {
                $("#pageList").html("")
            }
        }
    })
}


function resetDefault() {
    $(".selectedPageName").text('')
    $("#NumberOfCycles").val(0)
}
function selectedPageName() {
    var value = $("#pageList").val();
    if (value && value.length > 0)
        return value[0];

    return "";
}

$("#pageList").change(function () {
    var page = $(this).val();
    getPageInfo();
})

function getPageInfo() {
    var html = "";
    var displaySize = $("#pmgInch").val();
    var pageName = selectedPageName();
    $ajaxFunc("/PMG/GetCompositePageByName", { "name": pageName, "pageType": pageType }, function (res) {
        console.log("page:", res)
        if (res && !res.errorMsg) {
            $(".selectedPageName").text(res.pageName)
            $("#NumberOfCycles").val(res.numCycles)
            if (res.sequences && res.sequences.length > 0) {
                for (var i = 0; i < res.sequences.length; i++) {
                    var item = res.sequences[i];
                    html += '<tr>';
                    html += ' <td class="text-center"><input type="radio" name="compositeRadia" value="' + item.displayAlertType + '"  onclick="checkedOne(this,' + i + ')"/></td>';
                    html += ' <td class="text-center">' + (i + 1) + '</td>' +
                    ' <td class="text-center">' + item.startTime + '</td>' +
                    ' <td class="text-center">' + item.duration + '</td>' +
                    ' <td class="text-center">' + getAlertName(item.displayAlertType) + '</td>' +
                    ' <td class="text-center">' + item.filename + '</td>';
                    html += '</tr>';
                }
                $("#dataBind").html(html);
            }
        } else {
            resetDefault()
        }
    });
}

function checkedOne(self, i) {

}

$(function () {
    getCompositeList()
})



function layClose() {
    parent.layer.closeAll();
}


function getAlertName(type) {
    var text = "Display None";
    switch (type) {
        default:
        case 0:
            text = "Display None";
            break;
        case 1:
            text = "Display Speed";
            break;
        case 2:
            text = "Display Text";
            break;
        case 3:
            text = "Display Graphics";
            break;
        case 4:
            text = "Display Animation";
            break;
        case 5:
            text = "Display Time";
            break;
        case 6:
            text = "Display Temperature";
            break;


        case 129:
            text = "Flash Display";
            break;
        case 130:
            text = "Strobes";
            break;
        case 131:
            text = "Flash + Strobes";
            break;
        case 132:
            text = "Camera Trigger";
            break;


        case 133:
            text = "GPIO Port 1";
            break;
        case 134:
            text = "GPIO Port 2";
            break;
        case 135:
            text = "GPIO Port 3";
            break;
        case 136:
            text = "GPIO Port 4";
            break;

    }

    return text;
}
