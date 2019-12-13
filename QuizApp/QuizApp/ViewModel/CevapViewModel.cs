using QuizApp.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApp.ViewModel
{
    public class CevapViewModel
    {

        public IEnumerable<q_soru> Soru { get; set; }
        public q_secenek cevap { get; set; }
    }
}