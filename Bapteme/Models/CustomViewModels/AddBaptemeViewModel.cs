﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Bapteme.Models;

namespace Bapteme.Models.CustomViewModels
{
	public class AddBaptemeViewModel:AddUserViewModel
	{
		public Guid CelebrationId { get; set; }
	}
}
