using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bapteme.Models.ManageViewModels
{
	public class IndexViewModel
	{
		public bool HasPassword { get; set; }

		public IList<UserLoginInfo> Logins { get; set; }

		public string PhoneNumber { get; set; }

		public bool TwoFactor { get; set; }

		public bool BrowserRemembered { get; set; }

		public string Id { get; set; }

		[Display(Name = "Nom :")]
		public string LastName { get; set; }

		[Display(Name = "Prénom :")]
		public string FirstName { get; set; }

		[Display(Name = "Nom de naissance :")]
		public string BirthName { get; set; }

		[Display(Name = "Téléphone mobile:")]
		public string TelephonMobile { get; set; }

		[Display(Name = "Date de naissance:")]
		public DateTime BirthDate { get; set; }
    }
}
