using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeshBrain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LeshBrain.ViewModels
{
    public class MentorsSearchClientsViewModel
    {
        public IList<UserEntity> Users { get; set; }
        public ClientsFiltrViewModel filtrModel { get; set; }
        public MentorsSearchClientsViewModel()
        {
            Users = new List<UserEntity>();
        }
    }
}
