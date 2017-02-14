using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bapteme.Models
{
	public enum role { Viewer, Manager, Administrateur, Contact}
	public class UserParoisse : BaseEntity
	{
		[Key]
		public Guid Id { get; set; }

		public Guid ParoisseId { get; set; }
		[ForeignKey("ParoisseId")]
		public Paroisse Paroisse { get; set; }

		public string UserId { get; set; }

		public role Role { get; set; }
    }
}
