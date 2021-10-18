using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LeshBrain.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public List<Test> Tests { get; set; }
        public List<Topic> Topics { get; set; }
        public List<Book> Books { get; set; }
    }
}
