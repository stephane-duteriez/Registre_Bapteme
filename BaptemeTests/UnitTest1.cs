using System;
using Xunit;
using Bapteme.Controllers;
using Microsoft.AspNetCore.Mvc;
using Bapteme.Data;
using Microsoft.AspNetCore.Identity;
using Bapteme.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Principal;
using System.Threading;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Bapteme;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Authentication;
using System.Collections.Generic;

namespace BaptemeTests
{
    public class HomeControllerTests
    {
		private readonly IServiceProvider serviceProvider;

		private BaptemeDataContext _db;
		private ApplicationDbContext _dbUsers;
		private UserManager<ApplicationUser> _userManager;

		public HomeControllerTests()
		{
			var services = new ServiceCollection();

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseInMemoryDatabase());

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddDbContext<BaptemeDataContext>(options =>
			{
				options.UseInMemoryDatabase();
			});

			serviceProvider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

			_db = serviceProvider.GetRequiredService<BaptemeDataContext>();
			_dbUsers = serviceProvider.GetRequiredService<ApplicationDbContext>();
			_userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			if (_dbUsers.Users.CountAsync().Result == 0)
			{
				ApplicationUser new_user01 = new ApplicationUser { UserName = "UserWithOneParoisse" };
				ApplicationUser new_user02 = new ApplicationUser { UserName = "UserWithTwoParoisses" };
				ApplicationUser new_user03 = new ApplicationUser { UserName = "UserWithoutParoisse" };
				_userManager.CreateAsync(new_user01);
				_userManager.CreateAsync(new_user02);
				_userManager.CreateAsync(new_user03);
				Paroisse new_paroisse01= new Paroisse { Name = "Paroisse01" };
				Paroisse new_paroisse02 = new Paroisse { Name = "Paroisse02" };
				Paroisse new_paroisse03 = new Paroisse { Name = "Paroisse03" };
				_db.Paroisses.Add(new_paroisse01);
				_db.Paroisses.Add(new_paroisse02);
				_db.Paroisses.Add(new_paroisse03);
				_db.SaveChanges();
				_db.UserParoisse.Add(new UserParoisse { ParoisseId = new_paroisse01.Id, UserId = new_user01.Id });
				_db.UserParoisse.Add(new UserParoisse { ParoisseId = new_paroisse01.Id, UserId = new_user02.Id });
				_db.UserParoisse.Add(new UserParoisse { ParoisseId = new_paroisse02.Id, UserId = new_user02.Id });
				_db.SaveChanges();
			}
		}

		[Fact]
        public async Task HomeIndexUserAsOneParoisseTests()
        {
			// Arrange
			ApplicationUser new_user = await _dbUsers.Users.FirstOrDefaultAsync(p=>p.UserName == "UserWithOneParoisse" );
			HomeController controller = new HomeController(_db, _userManager, _dbUsers);
			GenericIdentity new_identity = new GenericIdentity(new_user.UserName);
			new_identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, new_user.Id));
			ClaimsPrincipal new_claim = new ClaimsPrincipal( new_identity);

			var mock = new Mock<IHttpContextAccessor>();
			mock.Setup(p => p.HttpContext.User).Returns(new_claim);
			controller.ControllerContext.HttpContext = mock.Object.HttpContext;

			// Act
			IActionResult result = await controller.Index() as IActionResult;

			// Assert			
			RedirectToActionResult resultAction = Assert.IsType<RedirectToActionResult>(result);

			Assert.Equal("Post", resultAction.ActionName);
			Assert.Equal("Paroisse", resultAction.ControllerName);
		}

		[Fact]
		public async Task HomeIndexUserNoIdTests()
		{
			// Arrange

			HomeController controller = new HomeController(_db, _userManager, _dbUsers);
			GenericIdentity new_identity = new GenericIdentity("");
			ClaimsPrincipal new_claim = new ClaimsPrincipal(new_identity);
			var mock = new Mock<IHttpContextAccessor>();
			mock.Setup(p => p.HttpContext.User).Returns(new_claim);
			controller.ControllerContext.HttpContext = mock.Object.HttpContext;

			// Act
			IActionResult result = await controller.Index() as IActionResult;

			// Assert
			ViewResult resultView = Assert.IsType<ViewResult>(result);
		}

		[Fact]
		public async Task HomeIndexUserAsTwoParoisseTests()
		{
			// Arrange
			ApplicationUser new_user = await _dbUsers.Users.FirstOrDefaultAsync(p=>p.UserName == "UserWithTwoParoisses");
			HomeController controller = new HomeController(_db, _userManager, _dbUsers);
			GenericIdentity new_identity = new GenericIdentity(new_user.UserName);
			new_identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, new_user.Id));
			ClaimsPrincipal new_claim = new ClaimsPrincipal(new_identity);

			var mock = new Mock<IHttpContextAccessor>();
			mock.Setup(p => p.HttpContext.User).Returns(new_claim);
			controller.ControllerContext.HttpContext = mock.Object.HttpContext;

			// Act
			IActionResult result = await controller.Index() as IActionResult;

			// Assert
			ViewResult resultView = Assert.IsType<ViewResult>(result);
			Assert.Equal("~/Views/Paroisse/Index.cshtml", resultView.ViewName);
			var model = Assert.IsType<List<Paroisse>>(resultView.Model);
			Assert.Equal(2, model.Count);
		}

		[Fact]
		public async Task HomeIndexUserWithoutParoisseTests()
		{
			// Arrange
			ApplicationUser new_user = await _dbUsers.Users.FirstOrDefaultAsync(p=>p.UserName=="UserWithoutParoisse");
			HomeController controller = new HomeController(_db, _userManager, _dbUsers);
			GenericIdentity new_identity = new GenericIdentity(new_user.UserName);
			new_identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, new_user.Id));
			ClaimsPrincipal new_claim = new ClaimsPrincipal(new_identity);

			var mock = new Mock<IHttpContextAccessor>();
			mock.Setup(p => p.HttpContext.User).Returns(new_claim);
			controller.ControllerContext.HttpContext = mock.Object.HttpContext;

			// Act
			IActionResult result = await controller.Index() as IActionResult;

			// Assert
			ViewResult resultView = Assert.IsType<ViewResult>(result);

			Assert.IsType<List<Paroisse>>(resultView.Model);
			var model = Assert.IsType<List<Paroisse>>(resultView.Model);
			Assert.Equal(3, model.Count);
		}
	}
}
