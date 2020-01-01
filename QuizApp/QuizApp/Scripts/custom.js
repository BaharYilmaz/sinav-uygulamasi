

$("#quizDogruCvp").hide();
    var dogruMu = false;

    var quizDogruCvp = $("#quizDogruCvp").text();


    $('.quizCevap').click(function () {     

        $('.quizCevap').css("color", "gray");
        $(".quizCevap").unbind("click");

      
        var cevap = $(this).attr('name');

        if (quizDogruCvp == cevap) {
            $(this).css("background-color", "LightGreen")
            dogruMu = true;
        }
        else {
            $(this).css("background-color", "LightCoral")
            dogruMu = false;
           

        }
      
        var btn = $("#btnNext");
        btn.removeAttr("disabled");
     
    });


$("#btnNext").click(function () {
    

        var soruUniq = $("#SoruUniq").text();
        var sayac_ = $("#sure").text();
        $.ajax({

            url: "/Quiz/QuizSonuc",
            type: 'POST',
            data: "&soruUniq=" + soruUniq + "&sonuc=" + dogruMu+"&sayac="+sayac_,
            success: function (gelenveri) {
              
            },
            error: function (hata) {

            }
        });
       
    });



