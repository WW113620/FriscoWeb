

$("#EnableRadar input[type='checkbox']").click(function () {
    var isCheck = this.checked;
    checkEnableRadar(isCheck)
});

function checkEnableRadar(isCheck) {
    if (isCheck) {
        $("input[name='RadarName'],input[name='Operation']").each(function () {
            $(this).attr("disabled", false);
        })
        $("#RadarHoldoverTime,#RadarUnitResolution,#RadarExternalRadarSpeed").attr("disabled", false);
        var val = $('input:radio[name="RadarName"]:checked').val();
        if (val != 3) {
            $("#RadarExternalEchoPanRadarData").attr("disabled", false);
        }

        $(".form-control-number-points").attr("disabled", false);
        if ($(".add1") && $(".add1").length > 0) {
            $(".add1").attr("class", "add")
            $(".reduce1").attr("class", "reduce")
            bindnumber();
        }

    } else {
        $("input[name='RadarName'],input[name='Operation']").each(function () {
            $(this).attr("disabled", true);
        })
        $("#RadarHoldoverTime,#RadarUnitResolution,#RadarExternalRadarSpeed,#RadarExternalEchoPanRadarData").val(0).attr("disabled", true);
        $(".form-control-number-points").val(0).attr("disabled", true);
        $(".add").attr("class", "add1")
        $(".reduce").attr("class", "reduce1")

    }
}

$('input:radio[name="RadarName"]').click(function () {
    var val = parseInt($(this).val());
    if (val == 1) {
        $("#InternalChecked,#OperationDirection").show();
        $("#ExternalChecked").hide();
    } else {
        $("#InternalChecked,#OperationDirection").hide();
        $("#ExternalChecked").show();
        if (val == 2) {
            $("#RadarExternalEchoPanRadarData").attr("disabled", false);
        }
        else {
            $("#RadarExternalEchoPanRadarData").val(0).attr("disabled", true);
        }

    }
});

function submit() {
    var PMGID = $("#hidSelectCurrentPMGID").val();
    if (!PMGID) {
        LayerMsg("Please select a device");
        return false;
    }

    var EnableRadar = $("#EnableRadar input[type='checkbox']:checked").val();
    if (!EnableRadar) {
        LayerMsg("Please checked Enable Radar");
        return false;
    }

    var Radar = $('input:radio[name="RadarName"]:checked').val();
    if (!Radar) {
        LayerMsg("Please checked Radar");
        return false;
    }

    var RadarOperationDirection = $('#OperationDirection input:radio[name="Operation"]:checked').val();
    var RadarHoldoverTime = $("#RadarHoldoverTime").val();
    var RadarCosine = $("#RadarCosine").val();
    var RadarUnitResolution = $("#RadarUnitResolution").val();
    var RadarSensitivity = $("#RadarSensitivity").val();
    var RadarTargetStrength = $("#RadarTargetStrength").val();
    var RadarTargetAcceptance = $("#RadarTargetAcceptance").val();
    var RadarTargetHoldOn = $("#RadarTargetHoldOn").val();

    var RadarExternalRadarSpeed = $("#RadarExternalRadarSpeed").val();
    var RadarExternalEchoPanRadarData = $("#RadarExternalEchoPanRadarData").val();

    if (Radar == 1) {
        RadarExternalRadarSpeed = 0;
        RadarExternalEchoPanRadarData = 0;

        if (RadarOperationDirection == undefined || RadarOperationDirection == null || RadarOperationDirection == "") {
            LayerMsg("Please checked Radar Operation Direction");
            return false;
        }

        if (RadarCosine < 0) {
            LayerMsg("Radar Cosine can't be less than 0");
            return false;
        }
        if (RadarCosine > 70) {
            LayerMsg("Radar Cosine Time can't be more than 70");
            return false;
        }

        if (RadarSensitivity < 1) {
            LayerMsg("Radar Sensitivity can't be less than 1");
            return false;
        }
        if (RadarSensitivity > 16) {
            LayerMsg("Radar Sensitivity Time can't be more than 16");
            return false;
        }

        if (RadarTargetStrength < 1) {
            LayerMsg("Radar Target Strength can't be less than 1");
            return false;
        }
        if (RadarTargetStrength > 99) {
            LayerMsg("Radar Target Strength can't be more than 99");
            return false;
        }

        if (RadarTargetAcceptance < 1) {
            LayerMsg("Radar Target Acceptance can't be less than 1");
            return false;
        }
        if (RadarTargetAcceptance > 100) {
            LayerMsg("Radar Target Acceptance can't be more than 100");
            return false;
        }

        if (RadarTargetHoldOn < 0) {
            LayerMsg("Radar Target Hold On can't be less than 0");
            return false;
        }
        if (RadarTargetHoldOn > 100) {
            LayerMsg("Radar Target Hold On can't be more than 100");
            return false;
        }


    } else {
        RadarOperationDirection = 0;
        RadarHoldoverTime = 0;
        RadarCosine = 0;
        RadarUnitResolution = 0;
        RadarSensitivity = 0;
        RadarTargetStrength = 0;
        RadarTargetAcceptance = 0;
        RadarTargetHoldOn = 0;
        if (Radar == 3) {
            RadarExternalEchoPanRadarData = 0;
        }
    }
    var jsonData = {
        "PMGID": PMGID, "Radar": Radar, "RadarHoldoverTime": RadarHoldoverTime, "RadarCosine": RadarCosine, "RadarUnitResolution": RadarUnitResolution,
        "RadarSensitivity": RadarSensitivity, "RadarTargetStrength": RadarTargetStrength, "RadarTargetAcceptance": RadarTargetAcceptance, "RadarTargetHoldOn": RadarTargetHoldOn,
        "RadarOperationDirection": RadarOperationDirection, "RadarExternalRadarSpeed": RadarExternalRadarSpeed, "RadarExternalEchoPanRadarData": RadarExternalEchoPanRadarData
    };

    $ajaxFunc("/PMG/SaveRadarData", jsonData, function (res) {
        LayerAlert(res.msg)
    });
}


function initData() {
    $ajaxFunc("/PMG/GetRadarData", {}, function (res) {
        console.log("res:", res)
        if (res.code == 0) {
            $("#EnableRadar input[type='checkbox']").prop('checked', true);
            $("#EnableRadar span").removeClass("checked").addClass("checked");
           

            $("#TrafficTargetStrength").val(res.model.TrafficTargetStrength)
            $("#TrafficMinimumTrackingDistance").val(res.model.TrafficMinimumTrackingDistance)
            $("#TrafficMinimumFollowingTime").val(res.model.TrafficMinimumFollowingTime)

            if (res.model.TrafficDataOnDemand == 0) {
                $("#TrafficDataOnDemand input[type='checkbox']").prop('checked', true);
                $("#TrafficDataOnDemand span").removeClass("checked").addClass("checked");
            } else {
                $("#TrafficDataOnDemand input[type='checkbox']").prop('checked', false);
                $("#TrafficDataOnDemand span").removeClass("checked");
            }

        }
    });
}

$(function () {
    checkEnableRadar(true)
    //initData();
})
