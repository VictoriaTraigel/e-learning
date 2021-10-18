using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeshBrain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeshBrain.ViewModels
{
    public class LibraryListViewModel
    {
        public SelectList ListCategory { get; set; }
        public string Name { get; set; }
        public IEnumerable<Book> Books { get; set; }
        public LibraryListViewModel()
        {

        }
    }
}
