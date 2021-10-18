using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LeshBrain.Models
{
    public class TestHistory
    {
        public int Id { get; set; }

        [Required]
        public double Result { get; set; } = 0;

        [Required]
        public int AmountRightAnswers { get; set; }

        [Required]
        public int MaxQuestion { get; set; }

        public string DatePass { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }

        public string UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
