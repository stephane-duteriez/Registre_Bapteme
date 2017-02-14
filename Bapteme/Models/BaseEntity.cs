using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bapteme.Models
{
    public class BaseEntity
    {
		public DateTime? DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
	}
}
