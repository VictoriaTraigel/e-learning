using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LeshBrain.Models
{
    public class UserEntity : IdentityUser<int>
    {
        public override bool EmailConfirmed { get; set; } = true;
        public override bool PhoneNumberConfirmed { get; set; } = true;

        public string ImageUrl { get; set; } = "/Content/UsersImage/spanch.jpg";

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronym { get; set; }
        public double Raiting { get; set; } = 0;
    }
}
