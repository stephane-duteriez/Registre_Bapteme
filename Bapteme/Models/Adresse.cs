using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bapteme.Models
{
	public class Adresse
	{
		[Key]
		public Guid Id { get; set; }

		[DataType(DataType.Text)]
		[StringLength(10, ErrorMessage = "Doit avoir un maximum de 10 charactères")]
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

		public string TelephoneFix { get; set; }
	}
}
