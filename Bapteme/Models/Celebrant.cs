using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bapteme.Models
{
	public class Celebrant
	{
		[Key]
		public Guid Id { get; set; }

		public Guid ParoisseId { get; set; }
		public string UserId { get; set; }

		[ForeignKey("ParoisseId")]
		public Paroisse Paroisse { get; set; }
	}
}
