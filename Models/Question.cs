using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeshBrain.Models
{
    public class Question
    {
        public int Id { get; set; }
        
        [Required]
        [Column(TypeName ="varchar(200)")]
        public string Content { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }

        public List<Answer> Answers { get; set; }
    }
}
