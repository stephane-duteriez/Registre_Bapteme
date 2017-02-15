using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bapteme.Models
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string BirthName { get; set; }
		public string TelephoneMobile { get; set; }
		public DateTime BirthDate { get; set; }
		public Guid CelebrationId { get; set; }
		public string numero_registre { get; set; }

		[DataType(DataType.Text)]
		[StringLength(500, ErrorMessage = "Nom du clocher doit avoir un maximum de 500 charactères")]
		public string Commentaire { get; set; }

		public Guid IdAdress { get; set; }

		public string DisplayName
		{
			get {
				return FirstName + " " + LastName;
			}
		}
	}
}
