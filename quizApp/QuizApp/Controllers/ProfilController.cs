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

      
        public ActionResult GrafikGoster()
        {
            var sinavlar =(db.q_sinavSonuc.Distinct()).ToList();
           // var dbSonSinav = ((from ss in db.q_sinavSonuc select ss.sinavNo).Distinct()).Max();
            return View(sinavlar);
        }

        public ActionResult AnlikGrafik()
        {
            List<string> sinavPuan = GetSinavSonuc();

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
            List<string> genelPuan = GetSinavSonuc();

            var chartSonuc = new Chart(500, 400);
            chartSonuc.AddTitle("Genel Sonuc Grafiği").AddLegend("Kategori")
                .AddSeries("Puan",
                xValue: new[] { "Anlatım Bozukluğu", "Ekler", "Yazım Kuralları", "Ses Bilgisi", "Sözcükte Anlam", "Sözcük Türleri" },
                yValues: genelPuan)
                .Write();

            return File(chartSonuc.ToWebImage().GetBytes(), "image/jpeg");
        }

        private List<string> GetSinavSonuc()
        {
            //TO DO: istenilen sınav sonucunu görme

            List<string> returnSonuc = new List<string>();
            var dbSonSinav = ((from ss in db.q_sinavSonuc select ss.sinavNo).Distinct()).Max();

            for (int i = 0; i < 6; i++)
            {
                var dbQuiz = (from ss in db.q_sinavSonuc
                              join k in db.q_kategori on ss.kategoriId equals k.kategoriId
                              where ss.sinavNo == dbSonSinav && ss.kategoriId == i

                              select new
                              {
                                  quizKategoriId = ss.kategoriId,
                                  quizPuan = ss.puan,

                              }).ToList();
                if (dbQuiz!=null)
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

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < dbSinavSayisi; j++)
                {
                    var dbQuiz = (from ss in db.q_sinavSonuc
                                  join k in db.q_kategori on ss.kategoriId equals k.kategoriId
                                  where ss.sinavNo == j && ss.kategoriId == i

                                  select new
                                  {
                                      quizKategoriId = ss.kategoriId,
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
                
            }

            return returnSonuc;

        }
    }
}