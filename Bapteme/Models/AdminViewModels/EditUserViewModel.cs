using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Bapteme.Models;

namespace Bapteme.Models.AdminViewModels
{
	public class EditUserViewModel
	{
		[Required]
		public ApplicationUser user { get; set; }

		public List<UserParoisse> userParoises { get; set; }

		public role role { get; set; }
    }
}
