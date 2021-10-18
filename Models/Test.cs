using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LeshBrain.Models
{
    public class Test
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int AmountQuestion { get; set; }
        public string Description { get; set; }
        public bool isFavorite { get; set; } = false;
        public bool isRequired { get; set; } = false;

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int TopicId { get; set; }
        public Topic Topic { get; set; }

        public List<Question> Questions { get; set; }
    }
}
