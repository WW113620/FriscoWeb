
$("#pmgInch").change(function () {
    resetDefault()
    getCompositeList();
})

function resetDefault() {
   
}

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

function getPageInfo()
{

}

$(function () {
    getCompositeList()
})



function layClose() {
    parent.layer.closeAll();
}