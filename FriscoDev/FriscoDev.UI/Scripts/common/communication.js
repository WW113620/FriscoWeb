function Add() {
    alert(55)
}

function Delete() {

}


var reg = /\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b/;
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

    var EthernetIPSetting = $("#EthernetIPSetting").val();
    if (EthernetIPSetting == 1) {
        var EthernetIPAddress = StrTrim($("#EthernetIPAddress").val())
        if (!reg.test(EthernetIPAddress))
        {
            LayerMsg("Please input valid Ethernet IP Address");
            return false;
        }
        var EthernetSubnetMask = StrTrim($("#EthernetSubnetMask").val())
        if (!reg.test(EthernetSubnetMask)) {
            LayerMsg("Please input valid Ethernet Subnet Mask");
            return false;
        }
        var EthernetDefaultGateway = StrTrim($("#EthernetDefaultGateway").val())
        if (!reg.test(EthernetDefaultGateway)) {
            LayerMsg("Please input valid Default Gateway");
            return false;
        }
    }

    var WIFIStationIPType = $("#WIFIStationIPType").val();
    if (WIFIStationIPType == 1) {
        var WIFIStationIPAddress = StrTrim($("#WIFIStationIPAddress").val())
        if (!reg.test(WIFIStationIPAddress)) {
            LayerMsg("Please input valid Wifi Station IP Address");
            return false;
        }
        var WIFIStationSubnetMask = StrTrim($("#WIFIStationSubnetMask").val())
        if (!reg.test(WIFIStationSubnetMask)) {
            LayerMsg("Please input valid Wifi Station Subnet Mask");
            return false;
        }
        var WIFIStationDefaultGateway = StrTrim($("#WIFIStationDefaultGateway").val())
        if (!reg.test(WIFIStationDefaultGateway)) {
            LayerMsg("Please input valid Wifi Station Default Gateway");
            return false;
        }
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

function initData() {
    
    $ajaxFunc("/PMG/GetCommunication", {}, function (res) {
        console.log("res:", res)
        if (res.code == 0) {
            for (var key in res.model) {
                if (key != "PMGID") {
                    $("#" + key).val(res.model[key]);
                }

            }

            onchangeType("#EthernetIPSetting", "changeEthernetIPSetting");
            onchangeType("#WIFIStationIPType", "changeWIFIStationIPType")

        }
    });
}


function defaultReset() {
    $("#Communication input").val("");
    $("#Communication select").val("0");
}

$(function () {
    initData();
    //var obj = { "name": "test", "password": "123456", "department": "IT", "old": 30 };
    //for (var key in obj) {
    //    console.log(key);    
    //    console.log(obj[key]); 
    //}
})

function onchangeType(self, id) {
    var val = $(self).val()
    if (val == 1) {
        $("#" + id + " input").attr('readonly', false)
    } else {
        $("#" + id + " input").val('')
        $("#" + id + " input").attr('readonly',true)
    }
}