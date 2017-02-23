using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bapteme.Models.CustomViewModels
{
	public class AddCelebrationViewModel : Celebration
	{
		public bool single_clocher { get; set; }

		public AddCelebrationViewModel()
		{
			single_clocher = false;
		}

		public Celebration Celebration {
			get
			{
				return new Celebration()
				{
					ClocherId = ClocherId,
					Date = Date
				};
			}
		}
    }
}
