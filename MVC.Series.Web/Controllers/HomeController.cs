using System.Web.Mvc;
using MVC.Series.ViewModel;

namespace MVC.Series.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var homeViewModel = new HomeViewModel();
			homeViewModel.PopulateClassicBooks();

			return View( homeViewModel );
		}

		[HttpPost]
		public ActionResult Index( HomeViewModel homeViewModel )
		{
			return View( homeViewModel );
		}

		public ActionResult CreateNewBook()
		{
			var bookViewModel = new BookViewModel();

			return PartialView( "~/Views/Shared/EditorTemplates/BookViewModel.cshtml", bookViewModel );
		}
	}
}