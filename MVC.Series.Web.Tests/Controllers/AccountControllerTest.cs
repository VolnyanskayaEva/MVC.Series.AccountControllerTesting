using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MVC.Series.Common;
using MVC.Series.Web.Controllers;
using MVC.Series.Web.Models;

namespace MVC.Series.Web.Tests.Controllers
{
	[TestClass]
	public class AccountControllerTest
	{
		private AccountController _accountController;

		[TestInitialize]
		public void Initialization()
		{
			// mocking HttpContext
			var request = new Mock<HttpRequestBase>();
			request.Expect( r => r.HttpMethod ).Returns( "GET" );
			var mockHttpContext = new Mock<HttpContextBase>();
			mockHttpContext.Expect( c => c.Request ).Returns( request.Object );
			var mockControllerContext = new ControllerContext( mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object );

			// mocking IAuthenticationManager
			var authDbContext = new ApplicationDbContext();
			var mockAuthenticationManager = new Mock<IAuthenticationManager>();
			mockAuthenticationManager.Setup( am => am.SignOut() );
			mockAuthenticationManager.Setup( am => am.SignIn() );

			var mockUrl = new Mock<UrlHelper>();

			var manager = new AccountManager<ApplicationUserManager, ApplicationDbContext, ApplicationUser>( authDbContext, mockAuthenticationManager.Object );
			_accountController = new AccountController( manager )
			{
				Url = mockUrl.Object,
				ControllerContext = mockControllerContext
			};

			// using our mocked HttpContext
			_accountController.AccountManager.Initialize( _accountController.HttpContext );
		}

		[TestMethod]
		public void AccountController_Register_UserRegistered()
		{
			var registerViewModel = new RegisterViewModel
			{
				Email = "test@test.com",
				Password = "123456"
			};

			// make sure user does not exist
			var user = _accountController.UserManager.FindAsync( registerViewModel.Email, registerViewModel.Password ).Result;
			if ( user != null )
			{
				var deleteResult = _accountController.UserManager.DeleteAsync( user ).Result;
			}

			var result = _accountController.Register( registerViewModel ).Result;

			Assert.IsTrue( result is RedirectToRouteResult );
			Assert.IsTrue( _accountController.ModelState.All(kvp => kvp.Key != "") );

			// remove the user after we're done
			user = _accountController.UserManager.FindAsync( registerViewModel.Email, registerViewModel.Password ).Result;
			if ( user != null )
			{
				var deleteResult = _accountController.UserManager.DeleteAsync( user ).Result;
			}
		}
	}
}
