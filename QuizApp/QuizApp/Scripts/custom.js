﻿
    $("#quizDogruCvp").hide();

    var dogruMu = false;

    var quizDogruCvp = $("#quizDogruCvp").text();
    // alert(quizDogruCvp);
    //alert("doğru cevabı alamadık");

    $('.quizCevap').click(function () {


        var cevap = $(this).attr('name');


        if (quizDogruCvp == cevap) {
            $(this).css("background-color", "green")
            dogruMu = true;
        }
        else {
            $(this).css("background-color", "red")
            // $("#quizDogruCvp").show();
            dogruMu = false;

        } var btn = $("#btnNext");
        btn.removeAttr("disabled");
        // $("#btnNext").prop("disabled", false);
        // window.alert(dogruMu);
    });

    //  nextClick = $("#soruSayac").text();
    // alert(nextClick);

    $("#btnNext").click(function () {

        //var sayac = Session["sayac"].ToString();
        //Session["sayac"] = int.Parse(sayac) + 1;
        //var a = Session["sayac"];
        //window.alert(sayac);

        var soruUniq = $("#SoruUniq").text();

        $.ajax({

            url: "/Quiz/QuizSonuc",
            //url: '@Url.Action("QuizSonuc", "Quiz")',
            type: 'POST',
            // dataType: "json",
            data: "&soruUniq=" + soruUniq + "&sonuc=" + dogruMu,
            success: function (gelenveri) {
              
            },
            error: function (hata) {
               // alert("hata");

            }
        });
        //nextClick++;
        //    alert("sayac --" + nextClick);

        //$("#soruSayac").text(nextClick);
        //$("#soruSayac").innerText = nextClick++;
    });


