using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeshBrain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeshBrain.ViewModels
{
    public class InfoTestViewModel
    {
        public SelectList ListCategory { get; set; }
        public Test Test { get; set; }
    }
}
