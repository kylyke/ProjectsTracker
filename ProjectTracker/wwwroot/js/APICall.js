$(document).ready(function () {


    $("#btnAPICall").click(function () {


        $(".overlay.popup").fadeIn();
        CallAPI();







    })
    function LoadingBar() {

        $("Fade_area").removeAttr("style");
        $("#Mymodal").removeAttr("style");



    }


    function CallAPI() {



        $.ajax({
            url: "",
            type: 'post',
            contentType: 'application/json',
            success: function () {


                setTimeout(function () { }, 10000);

            },

            success: function () {


                setTimeout(function () { }, 10000);

            }


        })

    }









})