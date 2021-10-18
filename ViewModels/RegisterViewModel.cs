using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LeshBrain.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name ="Username")]
        public string UserName { get; set; }

        //[Required]
        [DataType(DataType.Password)]
        [Display(Name ="Пароль")]
        public string Password { get; set; }

        //[Required]
        [DataType(DataType.Password)]
        [Display(Name ="Подтверждение пароля")]
        public string PasswordConfirm { get; set; }
        public string ReturnUrl { get; set; }
    }
}
