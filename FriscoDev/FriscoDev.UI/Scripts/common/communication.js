function Add() {
    alert(55)
}

function Delete() {

}

function defaultReset() {
    $("#Communication input").val("");
    $("#Communication select").val("0");
}

function Submit() {
    var PMGID = $("#hidSelectCurrentPMGID").val();
    if (!PMGID) {
        LayerMsg("Please select a device");
        return false;
    }

    var WirelessPIN = StrTrim($("#WirelessPIN").val());
    if (!WirelessPIN) {
        LayerMsg("Please input Wireless PIN");
        return false;
    }

    if (WirelessPIN.length > 6) {
        LayerMsg("Wireless PIN must be max 6 digits");
        return false;
    }

    var param = {};
    param.PMGID = PMGID;
    $("#Communication input[type=text],#Communication select").each(function () {
        if ($(this).attr("id"))
            param[$(this).attr("id")] = $(this).val();
    })

    console.log("param:", param)
    $ajaxFunc("/PMG/SaveCommunication", param, function (res) {
        LayerAlert(res.msg)
    });
}