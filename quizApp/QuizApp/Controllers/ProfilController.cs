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
        //private int? sinavNo = -1;

      
        public ActionResult GrafikGoster()
        {
            var sinavlar = (db.q_genelSonuc.Distinct()).ToList();
            // var dbSonSinav = ((from ss in db.q_sinavSonuc select ss.sinavNo).Distinct()).Max();
            return View(sinavlar);
        }

        public ActionResult AnlikGrafik(int? sinavNo)
        {
            List<string> sinavPuan = GetSinavSonuc(sinavNo);

            var chartSonuc = new Chart(500, 400);
            chartSonuc.AddTitle("Sınav Sonuc Grafiği").AddLegend("Kategori")
                .AddSeries("Puan",
                xValue: new[] { "Anlatım Bozukluğu", "Ekler", "Yazım Kuralları", "Ses Bilgisi", "Sözcükte Anlam", "Sözcük Türleri" },
                yValues: sinavPuan)
                .Write();

            return File(chartSonuc.ToWebImage().GetBytes(), "image/jpeg");

        }
        public ActionResult GenelGrafik()
        {
            var dbSonSinav = ((from ss in db.q_sinavSonuc select ss.sinavNo).Distinct()).Max();

            List<string> genelPuan = GetSinavSonuc(dbSonSinav);

            var chartSonuc = new Chart(500, 400);
            chartSonuc.AddTitle("Genel Sonuc Grafiği").AddLegend("Kategori")
                .AddSeries("Puan",
                xValue: new[] { "Anlatım Bozukluğu", "Ekler", "Yazım Kuralları", "Ses Bilgisi", "Sözcükte Anlam", "Sözcük Türleri" },
                yValues: genelPuan)
                .Write();

            return File(chartSonuc.ToWebImage().GetBytes(), "image/jpeg");
        }

        private List<string> GetSinavSonuc(int? Sinav_no)
        {
            //TO DO: istenilen sınav sonucunu görme

            //TO DO: istenilen sınav sonucunu görme

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
                                  //quizKategoriId = ss.kategoriId,
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

        private List<string> GetGenelSonuc()
        {
            //TO DO: ortalama sonuc

            List<string> returnSonuc = new List<string>();

            var dbSinavSayisi = (from ss in db.q_sinavSonuc select ss.sinavNo).Max();

            for (int i = 1; i < 7; i++)
            {
                //var m = 0;
                int? puanToplam = 0;
                //float sonuc =0.0f;
                for (int j = 1; j < dbSinavSayisi + 1; j++)
                {
                    //int? puan;
                    var dbQuiz = from ss in db.q_sinavSonuc
                                 join gs in db.q_genelSonuc on ss.sinavNo equals gs.quizCount
                                 where ss.sinavNo == j && ss.kategoriId == i

                                 select new
                                 {
                                     // quizKategoriId = ss.kategoriId,
                                     quizPuan = ss.puan,

                                 };

                    foreach (var puan in dbQuiz)
                    {
                        puanToplam += puan.quizPuan;

                    }
                    /*
                    if (m < dbSinavSayisi)
                    {
                        m++;
                    }
                    else break;*/

                    //puanlar.Add(dbQuiz.Take()
                    //var puanToplam1 = db.CurrentRow.Cells["Kategori"].Value.ToString();
                    //var e0 = liste.Select(x => new { Ad = x.Isim, IslemTip = x.Tip });
                    // puanToplam= dbQuiz.AsEnumerable().Select(q => q.quizPuan);
                    // puanToplam = dbQuiz.AsEnumerable().Sum(q => q.quizPuan);
                    //puanToplam += dbQuiz.AsEnumerable().Sum()
                    //puanToplam += dbQuiz.Take(quizPuan);
                    //var puanToplam2 = dbQuiz.AsQueryable().ToString(quizPuan);

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