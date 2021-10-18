using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LeshBrain.Models
{
    public class Answer
    {
        public int Id { get; set; }
        
        [Required]
        [Column(TypeName ="varchar(200)")]
        public string Content { get; set; }
        public bool RightAnswer { get; set; } = false;
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
