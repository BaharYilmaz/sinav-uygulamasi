using QuizApp.Models.EntityFramework;
using QuizApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace QuizApp.Controllers
{
    [Authorize(Roles = "S")]
    public class QuizController : Controller
    {
        private quizAppEntities db = new quizAppEntities();
        private static int soruSayac = 0;
        private static int sureSayac = 0;

        //Quiz Ekranını Görüntüleme
        [HttpGet]
        public ActionResult Quiz()
        {
            var dbgenelSonuc = new q_genelSonuc();
            var sinavNo = (from gs in db.q_genelSonuc select gs.quizCount).Max();


            if (sinavNo == null) dbgenelSonuc.quizCount = 1;
            else dbgenelSonuc.quizCount = sinavNo + 1;
            dbgenelSonuc.userid = 2;


            db.q_genelSonuc.Add(dbgenelSonuc);
            db.SaveChanges();

            sureSayac = 100;
            TempData["sayac"] = sureSayac;


            return RedirectToAction("QuizStart");

        }


        //Quiz cevabını veritabanına kaydetme
        [HttpPost]
        public ActionResult QuizSonuc(string soruUniq, bool sonuc, string sayac)
        {
            var soruID = Guid.Parse(soruUniq);
            var quizCount = (from gs in db.q_genelSonuc select gs.quizCount).Max();
            var sinavUniq = (from gs in db.q_genelSonuc where gs.quizCount == quizCount select gs.sinavUniq).First();


            q_sinavSonuc SinavSonuc = new q_sinavSonuc();
            q_soru dbsoru = db.q_soru.Where(s => s.soruUniq == soruID).SingleOrDefault();

            if (sonuc) dbsoru.derece = 1;
            else dbsoru.derece = -1;

            SinavSonuc.soruUniq = dbsoru.soruUniq;
            SinavSonuc.dogruMu = sonuc;

            if (sonuc) SinavSonuc.puan = 5;
            else SinavSonuc.puan = 0;

            SinavSonuc.kategoriId = dbsoru.kategoriId;
            SinavSonuc.sinavTarih = DateTime.Now;
            SinavSonuc.sinavNo = quizCount;
            SinavSonuc.sinavUniq = sinavUniq;

            sureSayac = int.Parse(sayac);

            db.q_sinavSonuc.Add(SinavSonuc);
            db.SaveChanges();

            return RedirectToAction("QuizStart");

        }

        //Quiz ekranına sırayla soru yazdırma
        public ActionResult QuizStart()
        {

            mesajViewModel mesajModel = new mesajViewModel();
            
            soruSayac++;

            while (soruSayac < 7)
            {


                var quiz = ((from k in db.q_kategori
                             join s in db.q_soru on k.kategoriId equals s.kategoriId
                             join sc in db.q_secenek on s.soruUniq equals sc.soruUniq
                             where s.derece == 0 && s.kategoriId == soruSayac

                             select new
                             {
                                 quizKategori = k.kategoriId,
                                 quizSoruUniq = s.soruUniq,
                                 quizSoru = s.soru,
                                 quizCvp1 = sc.cevap1,
                                 quizCvp2 = sc.cevap2,
                                 quizCvp3 = sc.cevap3,
                                 quizCvp4 = sc.cevap4,
                                 quizDogruCvp = sc.dogruCvp

                             }).Take(1)).ToList();

                if (quiz != null)
                {
                    var model = new quizSonucViewModel()
                    {
                        Soru = new q_soru(),
                        Secenek = new q_secenek()
                    };
                    model.Soru.soru = quiz[0].quizSoru;
                    model.Secenek.cevap1 = quiz[0].quizCvp1;
                    model.Secenek.cevap2 = quiz[0].quizCvp2;
                    model.Secenek.cevap3 = quiz[0].quizCvp3;
                    model.Secenek.cevap4 = quiz[0].quizCvp4;
                    model.Secenek.dogruCvp = quiz[0].quizDogruCvp;
                    model.Soru.soruUniq = quiz[0].quizSoruUniq;
                    model.Soru.kategoriId = quiz[0].quizKategori;


                    TempData["sayac"] = sureSayac;
                    return View(model);

                }
                else
                {
                    soruSayac++;
                }

            }

            mesajModel.Mesaj = "Sınav Tamamlandı...";
            mesajModel.Status = 1;
            mesajModel.LinkText = "Sınav sonucu için profile git";
            mesajModel.Url = "/Profil/GrafikGoster";


            return View("_mesaj", mesajModel);


        }

        //Quiz süresi bittiğinde mesaj sayfasına yönlendirme
        public ActionResult Route()
        {
            mesajViewModel mesajModel = new mesajViewModel();


            mesajModel.Mesaj = "Süre Bitti...";
            mesajModel.Status = 0;
            mesajModel.LinkText = "Anasayfaya git";
            mesajModel.Url = "/Home/Home";

            return View("_mesaj", mesajModel);


        }
    }

}



