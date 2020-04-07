

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
        var val=$('input:radio[name="RadarName"]:checked').val();
        if(val!=3)
        {
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

$(function () {
    checkEnableRadar(true)
})

$('input:radio[name="RadarName"]').click(function () {
    var val =parseInt($(this).val());
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