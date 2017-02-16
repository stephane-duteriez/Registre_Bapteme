using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Bapteme.Models;

namespace Bapteme.Models.CustomViewModels
{
    public class AddContactViewModel : AddUserViewModel
    {
		public string ChildId { get; set; }
		public bool useSameAdresse { get; set; }
		public RelationType RelationType { get; set; }
	}
}
