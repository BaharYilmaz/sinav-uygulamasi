using QuizApp.Models.EntityFramework;
using QuizApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizApp.Controllers
{
    [Authorize(Roles = "T")]
    public class QuizHazirlaController : Controller
    {
        quizAppEntities db = new quizAppEntities();

        //Quiz hazırlama ekranı
        public ActionResult QuizOlustur()
        {
            var model = new viewModel()
            {
                Kategori = db.q_kategori.ToList(),
                Soru = new q_soru(),
                Secenek = new q_secenek()

            };

       
            return View("QuizOlustur", model);

        }

        //Soru kaydetme
        [ValidateAntiForgeryToken]
        public ActionResult SoruKaydet(q_soru soru,q_secenek secenek)
        {
            mesajViewModel mesajModel = new mesajViewModel();

            if (!ModelState.IsValid)
            {
                var model_ = new viewModel()
                {
                    Kategori = db.q_kategori.ToList(),
                    Soru = new q_soru(),
                    Secenek = new q_secenek()

                };

                return View("QuizOlustur", model_);
            }
            else
            {
                soru.soruUniq = Guid.NewGuid();
                secenek.soruUniq = soru.soruUniq;
                soru.derece = 0;
                db.q_soru.Add(soru);
                db.q_secenek.Add(secenek);


            }
            db.SaveChanges();

            mesajModel.Mesaj = "Soru Başarıyla Eklendi...";
            mesajModel.Status = 1;
            mesajModel.LinkText = "Yeni Soru Ekle";
            mesajModel.Url = "/QuizHazirla/QuizOlustur";


            return View("_mesaj", mesajModel);
           

        }
        //Tüm soruları listeleme
        public ActionResult QuizListele()
        {
               
            var model = db.q_secenek.Include("q_soru").ToList();

            return View(model);
        }





    }
}