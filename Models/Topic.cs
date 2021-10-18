using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LeshBrain.Models
{
    public class Topic
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Test> Tests { get; set; }
    }
}
