using LeshBrain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeshBrain.ViewModels
{
    public class TestListViewModel
    {
        public SelectList ListCategory { get; set; }
        public string Name { get; set; }
        public IEnumerable<Test> Test { get; set; }
        public TestListViewModel()
        {

        }
    }
}
