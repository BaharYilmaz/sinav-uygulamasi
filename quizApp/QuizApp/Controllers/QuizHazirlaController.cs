using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizApp.Controllers
{
    [Authorize]
    public class QuizHazirlaController : Controller
    {
        // GET: QuizHazirla
        public ActionResult QuizOlustur()
        {
            return View();
        }
    }
}