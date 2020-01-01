using QuizApp.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace QuizApp.Controllers
{
    [Authorize(Roles = "S")]

    public class ProfilController : Controller
    {
        quizAppEntities db = new quizAppEntities();

        //Grafikler sayfasını açma
        public ActionResult GrafikGoster()
        {
            ViewBag.sinavNo = -1;
            var sinavlar = (db.q_genelSonuc.Distinct()).ToList();
            return View(sinavlar);
        
        }

        //Seçilen sınav numarasına göre grafik oluşturma
        public ActionResult GrafikGoster_(int? sinavNo)
        {
            ViewBag.sinavNo = sinavNo;
            var sinavlar = (db.q_genelSonuc.Distinct()).ToList();

            return View("GrafikGoster", sinavlar);

        }

        //Tek bir sınav sonucu grafiği oluşturma - default son sınav
        public ActionResult AnlikGrafik(int? sinavNo)
        {
            List<string> sinavPuan = GetSinavSonuc(sinavNo);

            var chartSonuc = new Chart(500, 400);
            chartSonuc.AddTitle("Sınav Sonuc Grafiği").AddLegend("Kategori")
                .AddSeries("Puan",
                xValue: new[] { "Anlatım Bozukluğu", "Ekler", "Yazım Kuralları", "Ses Bilgisi", "Sözcükte Anlam", "Sözcük Türleri" },
                yValues: sinavPuan)
                .Write();

            var grafik = File(chartSonuc.ToWebImage().GetBytes(), "image/jpeg");
            var sonuc = "<img src='"+ grafik + "'/>";
            return grafik;

        }
        //Tüm sınavların ortalama sonuc grafiği oluşturma
        public ActionResult GenelGrafik()
        {

            List<string> genelPuan = GetGenelSonuc();

            var chartSonuc = new Chart(500, 400);
            chartSonuc.AddTitle("Genel Sonuc Grafiği").AddLegend("Kategori")
                .AddSeries("Puan",
                xValue: new[] { "Anlatım Bozukluğu", "Ekler", "Yazım Kuralları", "Ses Bilgisi", "Sözcükte Anlam", "Sözcük Türleri" },
                yValues: genelPuan)
                .Write();

            return File(chartSonuc.ToWebImage().GetBytes(), "image/jpeg");
        }

        //Veritabanından istenilen sınav numarasına göre sonuc çekme
        private List<string> GetSinavSonuc(int? Sinav_no)
        {
           

            List<string> returnSonuc = new List<string>();

            int? dbSinav;
            if (Sinav_no == -1)
            {
                dbSinav = ((from ss in db.q_sinavSonuc select ss.sinavNo).Distinct()).Max();

            }
            else
            {
                dbSinav = Sinav_no;
            }

            for (int i = 1; i < 7; i++)
            {
                var dbQuiz = (from ss in db.q_sinavSonuc
                              join k in db.q_kategori on ss.kategoriId equals k.kategoriId
                              where ss.sinavNo == dbSinav && ss.kategoriId == i

                              select new
                              {
                                  quizPuan = ss.puan,

                              }).ToList();

                if (dbQuiz != null)
                {
                    var puanToplam = dbQuiz.AsEnumerable().Sum(q => q.quizPuan);
                    string b = puanToplam.ToString();
                    returnSonuc.Add(b);
                }
                else returnSonuc.Add(" ");

            }

            return returnSonuc;


        }

        //Veritabanından tüm sınav sınavların sonucuna göre ortalama sonuc çekme
        private List<string> GetGenelSonuc()
        {

            List<string> returnSonuc = new List<string>();

            var dbSinavSayisi = (from ss in db.q_sinavSonuc select ss.sinavNo).Max();

            for (int i = 1; i < 7; i++)
            {
                int? puanToplam = 0;
                for (int j = 1; j < dbSinavSayisi + 1; j++)
                {
                    var dbQuiz = from ss in db.q_sinavSonuc
                                 join gs in db.q_genelSonuc on ss.sinavNo equals gs.quizCount
                                 where ss.sinavNo == j && ss.kategoriId == i

                                 select new
                                 {
                                     quizPuan = ss.puan,

                                 };

                    foreach (var puan in dbQuiz)
                    {
                        puanToplam += puan.quizPuan;

                    }
                   

                }

                if (puanToplam != 0)
                {
                    puanToplam = puanToplam / dbSinavSayisi;
                    string b = puanToplam.ToString();
                    returnSonuc.Add(b);
                }
                else returnSonuc.Add(" ");


            }

            return returnSonuc;

        }
    }
}