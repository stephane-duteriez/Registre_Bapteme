using Bapteme.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bapteme.CustomFactory
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
	{
		public AppClaimsPrincipalFactory(
	   UserManager<ApplicationUser> userManager,
	   RoleManager<IdentityRole> roleManager,
	   IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
		{
		}

		public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
		{
			var principal = await base.CreateAsync(user);

			((ClaimsIdentity)principal.Identity).AddClaims(new[] {
			new Claim("UserId", user.Id),
		});

			return principal;
		}
	}
}
