using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bapteme.Models
{
	public class Celebration
	{
		[Key]
		public Guid Id { get; set; }

		public DateTime Date { get; set; }

		public Guid ClocherId { get; set; }
		[ForeignKey("ClocherId")]
		public Clocher Clocher { get; set; }

		public Guid CelebrantId { get; set; }
	}
}
