using LeshBrain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeshBrain.ViewModels
{
    public class TestComplPassViewModel
    {
        public Test Test { get; set; }
        public PageViewModel Passer { get; set; }
        public int CurrentResult { get; set; } = 0;
        public int MaxResult { get; set; } = 0;
        public TestComplPassViewModel()
        {
            Passer = new PageViewModel();
        }
        
    }
}
