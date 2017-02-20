using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bapteme.Models
{
	public enum isDemo { False, True};
	public class Paroisse : BaseEntity
	{
		public Paroisse()
		{
			Demo = isDemo.False;
		}

		[Key]
		public Guid Id { get; set; }

		public Guid ContactId { get; set; }

		[Display(Name = "Nom")]
		[Required]
		[DataType(DataType.Text)]
		[StringLength(50, ErrorMessage = "Nom de la paroisse doit avoir un maximum de 50 charactères")]
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

		public isDemo Demo { get; set; }
		public List<Clocher> Clochers { get; set; }
	}
}
