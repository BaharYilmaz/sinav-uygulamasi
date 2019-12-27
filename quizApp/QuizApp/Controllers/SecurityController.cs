using QuizApp.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QuizApp.Controllers
{
    public class SecurityController : Controller
    {
        //database adı/çağırma
        quizAppEntities db = new quizAppEntities();

        // GET: Security
        [AllowAnonymous]
        public ActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(q_kullanici kullanici)
        {
            //database içerisinde girilen kullanıcı adını ve şifreyi arama
            var user = db.q_kullanici.FirstOrDefault(x => x.username == kullanici.username && x.password == kullanici.password);

            if(user!= null)
            {
                FormsAuthentication.SetAuthCookie(user.username,false);//false - cookienin kalıcı olmaması için
                return RedirectToAction("Home", "Home");//giriş başarılı home a yönlendir

            }
            else
            {
                ViewBag.Mesaj= "Kullanıcı adı veya şifre geçersiz.";
                return View();
            }
            
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}