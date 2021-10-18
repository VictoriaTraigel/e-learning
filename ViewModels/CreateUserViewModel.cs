using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeshBrain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LeshBrain.ViewModels
{
    public class CreateUserViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronym { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ImageUrl { get; set; }= "/Content/UsersImage/spanch.jpg";

        public string UserName { get; set; }
        public string Password { get; set; }

        public List<IdentityRole<int>> AllRoles { get; set; }
        public IList<string> ListRoles { get; set; }
        public CreateUserViewModel()
        {
            ListRoles = new List<string>();
            AllRoles = new List<IdentityRole<int>>();
        }
    }
}
