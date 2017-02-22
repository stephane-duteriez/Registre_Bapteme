using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bapteme.Models.ManageViewModels
{
    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Le {0} avoir au moin {2} caractères et au maximum {1}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Noveau mot de passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer mot de passe")]
        [Compare("NewPassword", ErrorMessage = "Le nouveau mot de passe et la confirmation ne sont pas identiques.")]
        public string ConfirmPassword { get; set; }
    }
}
