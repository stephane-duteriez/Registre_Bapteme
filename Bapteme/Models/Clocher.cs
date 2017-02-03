using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bapteme.Models
{
    public class Clocher
    {
		[Key]
		public Guid Id { get; set; }

		[Display(Name = "Nom")]
		[Required]
		[DataType(DataType.Text)]
		[StringLength(50, ErrorMessage = "Nom du clocher doit avoir un maximum de 50 charactères")]
		public string Name { get; set; }
		private string _key;
		public string Key
		{
			get
			{
				if (_key == null)
				{
					_key = Regex.Replace(Name.ToLower(), "[^a-z0-9]", "-");
				}
				return _key;
			}
			set { _key = value; }
		}

		public Guid ParoisseId { get; set; }

		[ForeignKey("ParoisseId")]
		public Paroisse Paroisse { get; set; }

		public List<Clocher> Clochers { get; set; }
	}
}
