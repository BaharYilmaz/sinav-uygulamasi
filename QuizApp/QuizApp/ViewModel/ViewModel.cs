using QuizApp.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApp.ViewModel
{
    public class viewModel
    {

        public IEnumerable<q_kategori> Kategori { get; set; }
        public q_soru Soru { get; set; }
        public q_secenek Secenek { get; set; }

    }
}