using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeshBrain.Models;

namespace LeshBrain.ViewModels
{
    public class InfoBooksViewModel
    {
        public SelectList ListCategory { get; set; }
        public Book Book { get; set; }
    }
}
