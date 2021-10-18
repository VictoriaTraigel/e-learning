using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeshBrain.Models;
using LeshBrain.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LeshBrain.ViewModels
{
    public class UserViewModel
    {
        public UserEntity User { get; set; }
        public IList<string> UserRoles { get; set; }
        public UserViewModel()
        {
            User = new UserEntity();
            UserRoles = new List<string>();
        }
    }
}
