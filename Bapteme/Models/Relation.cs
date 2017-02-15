using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bapteme.Models
{
	public enum RelationType { Père, Mère, Frère, Soeur, Autre }
	public class Relation: BaseEntity
	{
		public Guid Id { get; set; }
		public string ParentId { get; set; }
		public string ChildId { get; set; }
		public RelationType RelationType {get; set;}
    }
}
