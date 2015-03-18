using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace MVC.Series.Common
{
	public class AccountManager<TUserManager, TDbContext, TUser>  : IDisposable
		where TUserManager : UserManager<TUser>
		where TDbContext : IdentityDbContext<TUser>
		where TUser : IdentityUser
	{
		private readonly IAuthenticationManager _authenticationManager;

		public TUserManager UserManager { get; set; }
		public HttpContextBase HttpContext { get; private set; }

		public AccountManager( TDbContext authContext, IAuthenticationManager authenticationManager )
			: this( AccountManagerFactory.Create<TUserManager, TDbContext, TUser>( authContext ) )
		{
			_authenticationManager = authenticationManager;
		}

		public AccountManager()
			: this( AccountManagerFactory.Create<TUserManager, TDbContext, TUser>() )
		{
		}

		public AccountManager( TUserManager userManager )
		{
			UserManager = userManager;
		}

		public void Initialize( HttpContextBase context )
		{
			HttpContext = context;
		}

		public IAuthenticationManager AuthenticationManager
		{
			get
			{
				return _authenticationManager ?? HttpContext.GetOwinContext().Authentication;
			}
		}

		public async Task SignInAsync( TUser user, bool isPersistent )
		{
			AuthenticationManager.SignOut( DefaultAuthenticationTypes.ExternalCookie );
			var identity = await UserManager.CreateIdentityAsync( user, DefaultAuthenticationTypes.ApplicationCookie );
			AuthenticationManager.SignIn( new AuthenticationProperties() { IsPersistent = isPersistent }, identity );
		}

		public void Dispose()
		{
			UserManager.Dispose();
			UserManager = null;
		}
	}

	internal static class AccountManagerFactory
	{
		internal static TUserManager Create<TUserManager, TDbContext, TUser>()
			where TUserManager : UserManager<TUser>
			where TDbContext : IdentityDbContext<TUser>
			where TUser : IdentityUser
		{
			return Create<TUserManager, TDbContext, TUser>( Activator.CreateInstance<TDbContext>() );
		}

		internal static TUserManager Create<TUserManager, TDbContext, TUser>( TDbContext context )
			where TUserManager : UserManager<TUser>
			where TDbContext : IdentityDbContext<TUser>
			where TUser : IdentityUser
		{
			var store = new UserStore<TUser>( context );
			var manager = (TUserManager)Activator.CreateInstance( typeof( TUserManager ), store );
			return manager;
		}
	}
}
