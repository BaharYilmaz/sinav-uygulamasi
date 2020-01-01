
setInterval(bak, 1000);

var sayac_ = $("#sure").text();
var sayac = parseInt(sayac_);

//alert(sayac);
//function bak() {
//    if (sayac > 0) {
//        sayac = sayac - 1;
//        $("#sure").text(sayac);
//    }
//    else {
//        $("#sure").text("süre bitti");
//    }
//}
var elem = document.getElementById("myBar");
function bak() {

    if (sayac > 0) {
        sayac = sayac - 1;
        $("#sure").text(sayac);
        elem.style.width = sayac + "%";
    }
    else {
        $(location).attr('href', 'Route');

    }
}
