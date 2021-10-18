using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeshBrain.ViewModels
{
    public class AccountSettingViewModel
    {
        public EditUserViewModel edit { get; set; }
        public ChangePasswordViewModel change { get; set; }
        public ChangeRoleViewModel changeRole { get; set; }
        public AccountSettingViewModel()
        {
            edit = new EditUserViewModel();
            change = new ChangePasswordViewModel();
            changeRole = new ChangeRoleViewModel();
        }
    }
}
