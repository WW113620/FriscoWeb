
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

var sequencesArray = [];
function getPageInfo() {
    var html = "";
    var displaySize = $("#pmgInch").val();
    var pageName = selectedPageName();
    $ajaxFunc("/PMG/GetCompositePageByName", { "name": pageName, "pageType": pageType }, function (res) {
        console.log("page:", res)
        $("#dataBind").html('');
        if (res && !res.errorMsg) {
            $(".selectedPageName").text(res.pageName)
            $("#NumberOfCycles").val(res.numCycles)
            if (res.sequences && res.sequences.length > 0) {
                sequencesArray = res.sequences;
                for (var i = 0; i < res.sequences.length; i++) {
                    var item = res.sequences[i];
                    html += '<tr>';
                    html += ' <td class="text-center"><input type="radio" name="compositeRadio" value="' +i + '" /></td>';
                    html += ' <td class="text-center no">' + (i + 1) + '</td>' +
                    ' <td class="text-center">' + item.startTime + '</td>' +
                    ' <td class="text-center">' + item.duration + '</td>' +
                    ' <td class="text-center" data-alertType="' + item.displayAlertType + '">' + getAlertName(item.displayAlertType) + '</td>' +
                    ' <td class="text-center">' + item.filename + '</td>';
                    html += '</tr>';
                }
                $("#dataBind").html(html);
            }
        } else {
            sequencesArray = [];
            resetDefault()
        }
    });
}

function checkedOne(self, i) {

}

$(function () {
    getCompositeList()
})

function AddSegment()
{
    layer.open({
        type: 1,
        title: 'Add Segment',
        shadeClose: true,
        area: ['430px', '400px'],
        content: $('#dialogComposite')
    });
}


function EditSegment() {
    var selected = $("#dataBind input[name='compositeRadio']:checked").val();
    if (selected == undefined || selected == null || selected == "") {
        LayerAlert("Please select a segment");
        return false;
    }
    layer.open({
        type: 1,
        title: 'Add Segment',
        shadeClose: true,
        area: ['430px', '400px'],
        content: $('#dialogComposite')
    });
}

function DeleteSegment() {
    var selected = $("#dataBind input[name='compositeRadio']:checked").val();
    if (selected == undefined || selected == null || selected == "") {
        LayerAlert("Please select a segment");
        return false;
    }

    layer.confirm("Are you sure to clear all?", {
        title: false, closeBtn: 0,
        btn: ['Yes', 'No']
    }, function (index) {
        layer.close(index);

        $('#dataBind tr:eq(' + selected + ')').remove();
        ResetTable()
    }, function () {

    });


}

function ResetTable()
{
    $("#dataBind tr input[name='compositeRadio']").each(function (i, val) {
        val.value = i;
    })

    $("#dataBind tr td.no").each(function (i, val) {
        $(this).text(i + 1);
    })
}

function createPage() {
    var displayType = $("#pmgInch").val();
    layer.prompt({ title: 'Enter Text Page Name', btn: ["Save", "Cancel"] }, function (val, index) {
        $ajaxFunc("/PMG/CreateNewPage", { "name": val, "displayType": displayType, "pageType": pageType }, function (res) {
            if (res.code == 0) {
                layer.close(index);
                $("#hidPageName").val(val)
                getCompositeList()
            } else {
                LayerAlert(res.msg);
            }
        });

    });
}

function ClearAll() {

    layer.confirm("Are you sure to clear all?", {
        title: false, closeBtn: 0,
        btn: ['Yes', 'No'] 
    }, function (index) {
        layer.close(index);
        $('#dataBind').html('');
    }, function () {
      
    });

}

function MoveUp() {

    var selected = $("#dataBind input[name='compositeRadio']:checked").val();
    if (selected == undefined || selected == null || selected == "") {
        LayerAlert("Please select a segment");
        return false;
    }
 
    var current = $('#dataBind tr:eq(' + selected + ')');
    var prev = current.prev();
    if (current.index() > 0) {
        current.insertBefore(prev); 
    }

    ResetTable()
}

function MoveDown() {

    var selected = $("#dataBind input[name='compositeRadio']:checked").val();
    if (selected == undefined || selected == null || selected == "") {
        LayerAlert("Please select a segment");
        return false;
    }
    var current = $('#dataBind tr:eq(' + selected + ')');
    var next = current.next(); 
    if (next) {
        current.insertAfter(next); 
    }
    ResetTable()
}


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


function deletePage() {
    var displaySize = $("#pmgInch").val();
    var pageName = selectedPageName();
    if (!pageName) {
        LayerAlert("Please select an composite page");
        return false;
    }

    layer.confirm("Are you sure to delete this page?", { title: false, closeBtn: 0, offset: 'auto' }, function (index) {
        layer.close(index);
        $ajaxFunc("/PMG/DeletePage", { "name": pageName, "pageType": pageType }, function (res) {
            if (res.code == 0) {
                LayerAlert("Delete successfully");
                getCompositeList()
                getPageInfo();
            } else {
                LayerAlert(res.msg);
            }
        });
    });

    
}


function savePage() {
    var displayType = $("#pmgInch").val();
    var pageName = selectedPageName();
    if (!pageName) {
        LayerAlert("Please select an composite page");
        return false;
    }

    var numCycles = $("#NumberOfCycles").val()
    if (numCycles < 0) {
        LayerAlert("Number Of Cycles can't be less than 0");
        return false;
    }
    if (numCycles > 1000) {
        LayerAlert("Number Of Cycles can't be more than 1000");
        return false;
    }

    var sequenceArray = [];
    $('#dataBind tr').each(function (i) {
        var sequence = {};
        sequence.startTime = $(this).children('td').eq(2).text();
        sequence.duration = $(this).children('td').eq(3).text();
        sequence.displayAlertType = $(this).children('td').eq(4).attr("data-alertType");
        sequence.filename = $(this).children('td').eq(5).text();
        sequenceArray.push(sequence);
    })


    var jsonData = {
        "pageName": pageName, "pageType": pageType, "displayType": displayType, "numCycles": numCycles, "sequenceStr": JSON.stringify(sequenceArray),
    };
    $ajaxFunc("/PMG/SaveCompositeOptions", jsonData, function (res) {
        if (res.code == 0) {
            LayerAlert("Save successfully");
        } else {
            LayerAlert(res.msg);
        }

    });

}