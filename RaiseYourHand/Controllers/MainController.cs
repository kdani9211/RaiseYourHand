using System.Reflection;
using System.Web.Mvc;

namespace RaiseYourHand.Controllers
{
	public class MainController : Controller
	{
		public ActionResult Participant(string room) =>
			this.View(model: room);
		public ActionResult Speaker(string room) =>
			this.View(model: room);
		public ActionResult Version() =>
			this.View(model: Assembly.GetExecutingAssembly());
	}
}