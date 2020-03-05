// input number click event
;(function(doc, win) {

    function handleResize() {
        win.onresize = null
    
        setTimeout(function () {
            win.onresize = handleResize
        }, 200);
    
        fitSize();
    }
    
    function fitSize() {
        $("#div-sidebar").css({
            height: $("#left_layout").height()
        })
    
        var screenWidth = screen.width
        if (screenWidth < 768) {
            $("#div-sidebar-right").css({
                width: screenWidth,
                height: 50,
                top: $("#header").height(),
                left: 0
            })
    
            $("#sidebar-right").css({
                border: "none"
            })
    
            $("#div-sidebar").css({
                height: $("#left_layout").height(),
                top: $("#header").height() + 60
            })
    
            $("#left_layout").css({
                position: "relative",
                top: 60
            })

            $("#bottom-bar").css({
                "margin-top": 65,
                "z-index": 2
            })

            $("button.button").addClass("block")
        } else {

            setTimeout(function () {
                $("#sidebar-right").css({
                    border: "1px solid #af9a9a"
                })
    
                $("#div-sidebar-right").css({
                    top: $("#header").height() + 15,
                    left: 120,
                    height: $("#left_layout").height() - 25,
                    width: 180
                })
                
                $("#div-sidebar").css({
                    top: $("#header").height() + 30,
                    height: $("#left_layout").height() - 25
                })

                $("#left_layout").css({
                    position: "relative",
                    top: 0
                })

                $("#bottom-bar").css({
                    "margin-top": 30
                })

                $("button.button").removeClass("block")
            }, 300);
        }

        
    }
    
    $(function() {
        $(".input-number-button .add").on("click", function (e) {
            var input = $(e.target).parent().parent().find("input[type=number]");
            var value = parseInt(input.val(), 10) || 0;
            if (value > 254)
                value = 254;
            input.val(value + 1);
        });
        
        $(".input-number-button .reduce").on("click", function (e) {
            var input = $(e.target).parent().parent().find("input[type=number]");
            var value = parseInt(input.val(), 10) || 0;
            if (value <1)
                value = 1;
            input.val(value - 1);
        });

        $("#showMoreMenu").on("click", function () {
            $("#mainContentTaps").toggleClass("more");
            if ($("#mainContentTaps").hasClass("more")) {
                $("#showMoreMenu").text("Show Less");
                Cookies.set("showMoreMenu", 1);
            } else {
                $("#showMoreMenu").text("Show More");
                Cookies.set("showMoreMenu", 0)
            }
        })

        var isShowMoreMenu = Cookies.get("showMoreMenu");
        if (isShowMoreMenu == 1) {
            $("#showMoreMenu").click();
        }
    
        fitSize();
        win.onresize = handleResize
    })
})(document, window);
