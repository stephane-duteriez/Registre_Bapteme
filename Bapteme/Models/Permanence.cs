using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bapteme.Models
{
	public enum JourSemaine { Lundi, Mardi, Mercredi, Jeudi, Vendredi, Samedi, Dimanche }

    public class Permanence : BaseEntity
	{
		[Key]
		public Guid Id { get; set; }

		public JourSemaine Jour { get; set; }
		public TimeSpan Debut { get; set; }
		public TimeSpan Fin { get; set; }

		public string Horraires {
			get {
				return String.Format("{0} : {1, 2}:{2, 2:00} à {3, 2}:{4, 2:00}", Jour, Debut.Hours, Debut.Minutes, Fin.Hours, Fin.Minutes);
			}
		}

		public Guid ClocherId { get; set; }

		[ForeignKey("ClocherId")]
		public Clocher Clocher { get; set; }
	}
}
