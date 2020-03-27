


function initSideBar() {
    layui.use('element', function () {
        var element = layui.element;
    });

    $(".fancy").uniform();
}

$(function () {

    initSideBar();

})

$("#sidebar .dropdown").click(function () {
    if ($(this).hasClass("active")) {
        $(this).removeClass("active").addClass("open");
    } else {
        $(this).removeClass("open").addClass("active");
    }
})

function ShowMenu(id) {
    if (id == 0) {
        $("#Quickid").hide();
        $("#Quickidmore").show();
        $("#Quickidmore").find("ul.layui-tab-title li").removeClass("layui-this");
        $("#Quickidmore").find("ul.layui-tab-title li").eq(0).addClass("layui-this");
        $("#Quickidmore").find("div.layui-tab-content .layui-tab-item").removeClass("layui-show");
        $("#Quickidmore").find("div.layui-tab-content .layui-tab-item").eq(0).addClass("layui-show");
    } else {
        $("#Quickid").show();
        $("#Quickidmore").hide();
        $("#Quickid").find("ul.layui-tab-title li").removeClass("layui-this");
        $("#Quickid").find("ul.layui-tab-title li").eq(0).addClass("layui-this");
        $("#Quickid").find("div.layui-tab-content .layui-tab-item").removeClass("layui-show");
        $("#Quickid").find("div.layui-tab-content .layui-tab-item").eq(0).addClass("layui-show");
    }
    initSideBar();
}

// bind time input
layui.use('laydate', function () {
    var laydate = layui.laydate;
    // Execute a new instance of laydate
    laydate.render({
        elem: '#StartTime',
        type: 'time',
        lang: 'en',
        theme: '#eb634d',
        format: 'HH:mm',
    });
});

// bind date input
layui.use('laydate', function () {
    var laydate = layui.laydate;
    // Execute a new instance of laydate
    laydate.render({
        elem: '#StartDate',
        lang: 'en',
        theme: '#eb634d',
        format: 'yyyy/MM/dd'
    });
});
layui.use('laydate', function () {
    var laydate = layui.laydate;
    // Execute a new instance of laydate
    laydate.render({
        elem: '#StopTime',
        type: 'time',
        lang: 'en',
        theme: '#eb634d',
        format: 'HH:mm',
    });
});

// bind date input
layui.use('laydate', function () {
    var laydate = layui.laydate;
    // Execute a new instance of laydate
    laydate.render({
        elem: '#StopDate',
        lang: 'en',
        theme: '#eb634d',
        format: 'yyyy/MM/dd'
    });
});
