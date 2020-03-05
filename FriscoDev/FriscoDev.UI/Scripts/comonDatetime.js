$(function () {
    layui.use('laydate',
        function () {
            var laydate = layui.laydate;
            //执行一个laydate实例
            laydate.render({
                elem: '#startDate',
                lang: 'en',
                type: 'datetime',
                format: 'yyyy-MM-dd HH:mm',
                theme: '#eb634d' //指定元素
            });
        });
    layui.use('laydate',
        function () {
            var laydate = layui.laydate;
            //执行一个laydate实例
            laydate.render({
                elem: '#endDate',
                lang: 'en',
                type: 'datetime',
                format: 'yyyy-MM-dd HH:mm',
                theme: '#eb634d' //指定元素
            });
        });
    layui.use('laydate',
        function () {
            var laydate = layui.laydate;
            //执行一个laydate实例
            laydate.render({
                elem: '#startDateLocation',
                lang: 'en',
                type: 'datetime',
                format: 'yyyy-MM-dd HH:mm',
                theme: '#eb634d' //指定元素
            });
        });
    layui.use('laydate',
        function () {
            var laydate = layui.laydate;
            //执行一个laydate实例
            laydate.render({
                elem: '#endDateLocation',
                lang: 'en',
                type: 'datetime',
                format: 'yyyy-MM-dd HH:mm',
                theme: '#eb634d' //指定元素
            });
        });
});