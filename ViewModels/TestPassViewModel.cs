using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeshBrain.Models;

namespace LeshBrain.ViewModels
{
    public class TestPassViewModel
    {
        public Test Test { get; set; }
        public Question Questions { get; set; }
        public List<Answer> Answers { get; set; }
        public PageViewModel Passer { get; set; }
        public TestPassViewModel()
        {
            Questions = new Question();
            Answers = new List<Answer>();
            Passer = new PageViewModel();
        }
    }
}
