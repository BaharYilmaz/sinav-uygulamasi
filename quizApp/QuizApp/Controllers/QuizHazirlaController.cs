using QuizApp.Models.EntityFramework;
using QuizApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizApp.Controllers
{
    [Authorize]
    public class QuizHazirlaController : Controller
    {
        quizAppEntities db = new quizAppEntities();

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
        public ActionResult SoruKaydet(q_soru soru,q_secenek secenek)
        {
            ////if (!ModelState.IsValid)
            ////{
            ////    var model = new viewModel()
            ////    {
            ////        Kategori = db.q_kategori.ToList(),
            ////        Soru = new q_soru(),
            ////        Secenek=new q_secenek()
                    
            ////    };

            ////    return View("QuizOlustur", model);
            ////}
            ////else 
            ////{
                soru.soruUniq = Guid.NewGuid();
                secenek.soruUniq = soru.soruUniq;
                soru.derece = 0;
               
                db.q_soru.Add(soru);
                db.q_secenek.Add(secenek);


            //}
            db.SaveChanges();
            var model = new viewModel()
            {
                Kategori = db.q_kategori.ToList(),
                Soru = new q_soru(),
                Secenek = new q_secenek()

            };
            return View("QuizOlustur", model);
         

        }
       


        [HttpPost]
        public ActionResult ResimYukle(HttpPostedFileBase imgInp)
        {
            string filePath = "";
            if (imgInp.ContentLength > 0)
            {
                filePath = Path.Combine(Server.MapPath("~/Content/images"), Guid.NewGuid().ToString() + "_" + Path.GetFileName(imgInp.FileName));
                imgInp.SaveAs(filePath);

            }
            ViewBag.resim = filePath;
            return RedirectToAction("QuizOlustur");
        }
    }
}