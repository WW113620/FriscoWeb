
var map; //google map 

var arrContentString = [];
function loadAllMap(data, mapDiv) {

    var london = new google.maps.LatLng(data.Positions[0].x, data.Positions[0].y);
    if (!data)
        london = new google.maps.LatLng('31.011763779271497', '-86.67888006878661');

    var curZoomLevel = data.ZoomLevel;
    if (!curZoomLevel) {
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
    for (var i = 0; i < data.Positions.length; i++) {
        var item = data.Positions[i];
        var direction = item.direction;
        var color = item.s;
        var colorName = color + ' ' + ConvertString(direction);
        var marker = new google.maps.Marker({
            map: map,
            icon: "/Images/ICON/" + colorName + ".png",
            position: new google.maps.LatLng(item.x, item.y),
            title: "Device: " + item.name + "\r\n" + "Direction: " + direction + "\r\n" + "Location: [" + item.x.toFixed(6) + "," + item.y.toFixed(6) + "]"
        });

        var imsi = item.imsi;
        var address = item.address;
        var contentString = '<div class="mapContent">' +
                             '<h3 style="color: black;"><b>Device: </b>' + item.name + '</h3>' +
                             '<div >' +
                             '<p style="color: black;"><b>Direction:</b> ' + direction + '</p>' +
                              '<p style="color: black;"><b>Address:</b> ' + address + '</p>';
                             //'<p><a href="/Home/location?imis=' + imsi + '"> View history location data on map</a> </p>';
        contentString += '</div>' +
                             '</div>';
        arrContentString.push(contentString);
        attachSecretMessage(marker, i);//所以这里传值就不用传map值了
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
