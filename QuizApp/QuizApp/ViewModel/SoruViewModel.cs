using QuizApp.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApp.ViewModel
{
    public class SoruViewModel
    {

        public IEnumerable<q_kategori> Kategori { get; set; }
        public IEnumerable<q_soru> Soru { get; set; }
        public IEnumerable<q_secenek> Secenek { get; set; }

    }
}