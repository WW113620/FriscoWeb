
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
    var displaySize = $("#pmgInch").val();
    var pageName = selectedPageName();
    $ajaxFunc("/PMG/GetCompositePageByName", { "name": pageName, "pageType": pageType }, function (res) {
    console.log("page:", res)
    if (res && !res.errorMsg) {
       
    } else {
        resetDefault()
    }
});
}

$(function () {
    getCompositeList()
})



function layClose() {
    parent.layer.closeAll();
}

