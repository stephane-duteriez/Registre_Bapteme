using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Bapteme.Models;

namespace Bapteme.Models.CustomViewModels
{
    public class AddUserViewModel
    {
		[Display(Name = "Prénom")]
		public string FirstName { get; set; }

		[Display(Name = "Nom")]
		public string LastName { get; set; }

		[Display(Name = "Email")]
		public string Email { get; set; }

		[Display(Name = "Téléphone Mobile")]
		public string TelephoneMobile { get; set; }

		[DataType(DataType.Text)]
		[StringLength(10, ErrorMessage = "Doit avoir un maximum de 10 charactères")]
		[Display(Name = "N°")]
		public string Numero { get; set; }

		[DataType(DataType.Text)]
		[StringLength(50, ErrorMessage = "Doit avoir un maximum de 50 charactères")]
		public string Rue { get; set; }

		[DataType(DataType.Text)]
		[StringLength(10, ErrorMessage = "Doit avoir un maximum de 10 charactères")]
		public string CP { get; set; }

		[DataType(DataType.Text)]
		[StringLength(10, ErrorMessage = "Doit avoir un maximum de 10 charactères")]
		public string Ville { get; set; }

		[DataType(DataType.Text)]
		[StringLength(100, ErrorMessage = "Doit avoir un maximum de 100 charactères")]
		public string Complement { get; set; }

		[Display(Name = "Téléphone Fix")]
		public string TelephoneFix { get; set; }

		[Display(Name = "Date Naissance")]
		public DateTime BirthDate { get; set; }
	}
}
