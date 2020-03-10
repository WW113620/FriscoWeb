var map; //google map 
function HomeControl(controlDiv, map) {
    var img = document.createElement('img');
    img.setAttribute("src", "../img/header.png");
    img.style.width = "70px";
    img.style.height = "70px";
    controlDiv.appendChild(img);
}
function resizeMap() {
    setTimeout(function () {
        google.maps.event.trigger(map, 'resize');
    }, 500);
}

function loadMap(imsi, x, y, mapDiv, markericon, data) {

    var devInfo = data.devInfo;
    var direction = data.direction;
    var deviceType = data.devicetype;
    var address = data.address;

    var london = new google.maps.LatLng(x, y);

    var curZoomLevel = getStorageItem("zoomlevel");
    if (curZoomLevel == "") {
        curZoomLevel = 13;
    }

    var mapProp = {
        center: london,
        zoom: parseInt(curZoomLevel),
        mapTypeControl: false,
        mapTypeId: google.maps.MapTypeId.SATELLITE
    };
    map = new google.maps.Map(mapDiv, mapProp);
    var homeControlDiv = document.createElement('div');
    var homeControl = new HomeControl(homeControlDiv, map);

    map.controls[google.maps.ControlPosition.TOP_LEFT].push(homeControlDiv);
    var colorName = markericon + ' ' + ConvertString(direction);
    var marker = new google.maps.Marker({
        map: map,
        icon: "../img/ICON/" + colorName + ".png",
        position: london,
        draggable: true,
        title: "Device: " + devInfo + "\r\n" + "Direction: " + direction + "\r\n" + "Location: [" + parseFloat(x).toFixed(6) + "," + parseFloat(y).toFixed(6) + "]"
    });
    var contentStringa = '<div id="content">' +
        '<div id="siteNotice">' +
        '</div>' +
        '<h3 id="firstHeading" class="firstHeading" style="color: black;">Device: ' + devInfo + '</h3>' +
        '<div id="bodyContent">' +
        '<p style="color: black;"><b>Direction: </b>' + direction + '</p>' +
        '<p style="color: black;"><b>Address:</b> ' + address + '</p>' +
        '<p><a href="/Home/location?imis=' + imsi + '"> View history location data on map</a> </p>';
    contentStringa += '</div>' +
        '</div>';
    var infowindow = new google.maps.InfoWindow({
        content: contentStringa
    });

    google.maps.event.addListener(marker, 'click',
        function () {
            infowindow.open(map, marker);
        });

    google.maps.event.addListener(marker, 'dragend', function (MouseEvent) {

        if (confirm("Do you want to save this change?")) {
            var devId = $("#PId").val()
            $.getJSON("/Home/SaveDevicePosition?pmdid=" + $("#PmdId").val() + "&pid=" + devId + "&x=" + MouseEvent.latLng.lat() + "&y=" + MouseEvent.latLng.lng(), null, function (data) {
                alert("Saved Successfully!");
                // window.location.href = "/Home/Index?yew=" + new Date().getSeconds();
            });
        }
    });

    var timeout = 0;
    google.maps.event.addListener(map, 'zoom_changed', function () {
        var lastLevel = getStorageItem("zoomlevel");
        var currentLevel = map.getZoom();
        if (!currentLevel || parseInt(lastLevel) !== currentLevel) {
            clearTimeout(timeout);
            timeout = setTimeout(function () {
                $.get("/user/savezoom?level=" + currentLevel, null, function (data) {
                    setStorageItem("zoomlevel", currentLevel);
                });
            }, 5000);
        }
    });
}

var arrContentString = [];
function loadAllMap(positions, mapDiv) {
    var london = new google.maps.LatLng(positions[0].x, positions[0].y);
    var curZoomLevel = getStorageItem("zoomlevel");
    if (curZoomLevel == "") {
        curZoomLevel = 13;
    }
    var defaultZoom = parseInt(curZoomLevel);
    var mapProp = {
        center: london,
        zoom: parseInt(curZoomLevel),
        mapTypeControl: false,
        mapTypeId: google.maps.MapTypeId.SATELLITE
    };
    map = new google.maps.Map(mapDiv, mapProp);

    var markers = [];
    for (var i = 0; i < positions.length; i++) {
        var _arr = [positions[i].s, positions[i].name];
        var direction = positions[i].direction;
        var colorName = _arr[0] + ' ' + ConvertString(direction);
        var marker = new google.maps.Marker({
            map: map,
            icon: "../img/ICON/" + colorName + ".png",
            position: new google.maps.LatLng(positions[i].x, positions[i].y),
            title: "Device: " + _arr[1] + "\n" + "Direction: " + direction + "\n" + "Location: [" + positions[i].x.toFixed(6) + "," + positions[i].y.toFixed(6) + "]"
        });
        var imsi = positions[i].imsi;
        var deviceType = parseInt(positions[i].t) + 1;
        var address = positions[i].address;
        var contentString = '<div id="content">' +
            '<div id="siteNotice">' +
            '</div>' +
            '<h3 id="firstHeading" class="firstHeading" style="color: black;"><b>Device: </b>' + _arr[1] + '</h3>' +
            '<div id="bodyContent">' +
            '<p style="color: black;"><b>Direction:</b> ' + direction + '</p>' +
            '<p style="color: black;"><b>Address:</b> ' + address + '</p>' +
            '<p><a href="/Home/location?imis=' + imsi + '"> View history location data on map</a> </p>';
        contentString += '</div>' +
            '</div>';
        arrContentString.push(contentString);
        attachSecretMessage(marker, i);
        markers.push(marker);
    }

    var i = markers.length;
    var bounds = new google.maps.LatLngBounds();
    while (i--) {
        bounds.extend(new google.maps.LatLng(markers[i].getPosition().lat(), markers[i].getPosition().lng()));
    }
    map.fitBounds(bounds);

    //地图缩放时触发，当地图的缩放比例大于默认比例时，恢复为默认比例
    google.maps.event.addListener(map, 'zoom_changed', function () {
        if (map.getZoom() > defaultZoom) {
            map.setZoom(defaultZoom);
        }
    });
}

var arrHistoryDataContent = [];
function loadHistoryData(positions, mapDiv, type) {
    arrHistoryDataContent = [];
    var london = new google.maps.LatLng(positions[0].x, positions[0].y);
    var curZoomLevel = getStorageItem("zoomlevel");
    if (curZoomLevel == "") {
        curZoomLevel = 13;
    }
    var markers = [];

    var mapProp = {
        center: london,
        zoom: parseInt(curZoomLevel),
        panControl: true,
        zoomControl: true,
        mapTypeControl: true,
        scaleControl: true,
        streetViewControl: true,
        overviewMapControl: true,
        rotateControl: true,
        mapTypeId: google.maps.MapTypeId.SATELLITE
    };
    map = new google.maps.Map(mapDiv, mapProp);
    var homeControlDiv = document.createElement('div');

    map.controls[google.maps.ControlPosition.TOP_LEFT].push(homeControlDiv);

    for (var i = 0; i < positions.length; i++) {
        var item = positions[i];
        var direction = item.x.toFixed(6) + "," + item.y.toFixed(6);
        var address = item.address;
        var date = item.startDate + " —— " + item.endDate;
        var marker = new google.maps.Marker({
            map: map,
            icon: "../img/marker_red.png",
            position: new google.maps.LatLng(item.x, item.y),
            title: "Device: " + item.name + "\r\n" + "Address: " + address + "\r\n" + "Date: " + date + ""
        });

        var contentString = '<div id="content">' +
            '<div id="siteNotice">' +
            '</div>' +
            '<h3 id="firstHeading" class="firstHeading" style="color: black;">Device: ' + item.name + '</h3>' +
            '<div id="bodyContent">' +
            '<p style="color: black;"><b>Location: </b>[' + direction + ']</p>' +
            // '<p style="color: black;"><b>Address:</b> ' + address + '</p>' +
            '<p style="color: black;"><b>Date: </b>' + date + '</p>' +
            //'<p><select class="form-control" id="reportType" style="color: black;width:200px;">' +
            //                   ' <option value="1">Real Time Charts</option>' +
            //                   ' <option value="2">Time vs Count</option>' +
            //                   ' <option value="3">Enforcement Pie</option>' +
            //                   '<option value="4">Speed vs Count</option>' +
            //                   '<option value="5">Count vs Speed Percentile</option>' +
            //                   '<option value="6">Weekly Count vs Time</option>' +
            //                   '<option value="7">Enforcement Schedule</option>' +
            //               ' </select></p>' +
            //'<p><a href="#" onclick="ViewDataReport(\'' + item.x + '\',\'' + item.y + '\',\'' + item.IMSI + '\',\'' + item.PMDID + '\',\'' + item.startDate + '\',\'' + item.endDate + '\')">View Reports</a> </p>';
            '<p style="text-align:center;"><a href="#" onclick="ViewReports(\'' + item.x + '\',\'' + item.y + '\',\'' + item.IMSI + '\',\'' + item.PMDID + '\',\'' + item.startDate + '\',\'' + item.endDate + '\')">View Reports</a> </p>';
        contentString += '</div>' +
            '</div>';
        arrHistoryDataContent.push(contentString);
        attachLocationData(marker, i);
        markers.push(marker);
    }

    var i = positions.length;
    var bounds = new google.maps.LatLngBounds();
    while (i--) {
        bounds.extend(new google.maps.LatLng(markers[i].getPosition().lat(), markers[i].getPosition().lng()));
    }
    map.fitBounds(bounds);
}

function attachLocationData(marker, number) {
    var infowindow = new google.maps.InfoWindow(
        {
            content: arrHistoryDataContent[number],
            size: new google.maps.Size(50, 50)
        });
    google.maps.event.addListener(marker, 'click', function () {
        infowindow.open(map, marker);
    });
}


function attachSecretMessage(marker, number) {
    var infowindow = new google.maps.InfoWindow(
        {
            content: arrContentString[number],
            size: new google.maps.Size(50, 50)
        });
    google.maps.event.addListener(marker, 'click', function () {
        infowindow.open(map, marker);
    });
}
function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
function layClose1() {
    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
    parent.layer.close(index);
}
function ViewReports(x, y, IMSI, PMDID, startDate, endDate) {
    var dataUrl = ViewDataUrlNew(x, y, IMSI, PMDID, startDate, endDate);
    var layOpen = layer.open({
        type: 2,
        maxmin: true,
        title: "Report",
        shade: 0,
        //offset: ['10%', '45%'],
        area: ['900px', '620px'],
        content: [dataUrl, 'no']
    });
}
function layClose() {
    parent.layer.closeAll();
}

function ViewDataUrlNew(x, y, IMSI, PMDID, startDate, endDate) {
    var toUrl = ""
    var data = "xvalue=" + encodeURI(x) + "&yvalue=" + encodeURI(y) + "&pid=" + IMSI + "&pmdid=" + PMDID + "&startDate=" + encodeURI(startDate) + "&endDate=" + encodeURI(endDate);
    var domain = "/Dialog/Report?" + data;
    return domain;
}
function ViewDataUrl(x, y, IMSI, PMDID, startDate, endDate, reportType) {
    var toUrl = ""
    var data = "xvalue=" + encodeURI(x) + "&yvalue=" + encodeURI(y) + "&pid=" + IMSI + "&pmdid=" + PMDID + "&startDate=" + encodeURI(startDate) + "&endDate=" + encodeURI(endDate);
    switch (reportType) {
        default:
        case 1:
            toUrl = "/Dialog/Index?";
            break;
        case 2:
            toUrl = "/Dialog/TimeCount?";
            break;
        case 3:
            toUrl = "/Dialog/Pie?";
            break;
        case 4:
            toUrl = "/Dialog/SpeedCount?";
            break;
        case 5:
            toUrl = "/Dialog/CountSpeedPercentile?";
            break;
        case 6:
            toUrl = "/Dialog/WeeklyCountTime?";
            break;
        case 7:
            toUrl = "/Dialog/EnforcementSchedule?";
            break;
    }
    var domain = toUrl + data;
    return domain;
}

function ConvertString(dir) {
    var x = "";
    switch (dir) {
        default:
        case "":
        case "None":
        case "North":
            x = "N";
            break;
        case "North East":
            x = "NE";
            break;
        case "East":
            x = "E";
            break;
        case "South East":
            x = "SE";
            break;
        case "South":
            x = "S";
            break;
        case "South West":
            x = "SW";
            break;
        case "West":
            x = "W";
            break;
        case "North West":
            x = "NW";
            break;
    }
    return x;
}
