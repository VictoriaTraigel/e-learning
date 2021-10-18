using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeshBrain.ViewModels
{
    public class ChangePasswordViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string NewPassword { get; set; }
    }
}
