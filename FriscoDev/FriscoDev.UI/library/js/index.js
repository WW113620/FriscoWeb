/**
 * Created by richard on 2016/2/1.
 */
var html = document.getElementsByTagName('html')[0];
if(html){
    var w = window.innerWidth;
    var fontSize = (w>750?750:w)/750 * 100;
    html.style.fontSize = fontSize + 'px';
}
window.onload = function(){
    window.onresize = function(){
        var w = window.innerWidth;
        var fontSize = (w>750?750:w)/750 * 100;
        html.style.fontSize = fontSize + 'px';
    }
}


$(function () {
    $('.web-common-nav-btn').on('click',function () {
        $('.web-common-nav-list').slideToggle();
    })
    $('.web-common-nav-list .title').on('click',function () {
        $(this).parent().parent().siblings().find('.ul_list').slideUp()
        $(this).siblings().slideToggle();
    })
    $('.web-common-nav-list .l_title').on('click',function () {
        $(this).parent().siblings().slideToggle();
    })
})