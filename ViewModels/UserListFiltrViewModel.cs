using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeshBrain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeshBrain.ViewModels
{
    public class UserListFiltrViewModel
    {
        public List<UserViewModel> Users { get; set; }
        public SelectList RolesList { get; set; }
        public string UserName { get; set; }
        public UserListFiltrViewModel()
        {
            Users = new List<UserViewModel>();
        }
    }
}
