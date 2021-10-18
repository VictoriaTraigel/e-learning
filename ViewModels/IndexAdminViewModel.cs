using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeshBrain.Models;

namespace LeshBrain.ViewModels
{
    public class IndexAdminViewModel
    {
        public int AmountClients { get; set; } = 0;
        public int AmountCompany { get; set; } = 0;
        public int AmountAnons { get; set; } = 0;
        public List<Test> Tests { get; set; }
        public List<UserViewModel> Users { get; set; }
        public IndexAdminViewModel()
        {
            Tests = new List<Test>();
            Users = new List<UserViewModel>();
        }
    }
}
