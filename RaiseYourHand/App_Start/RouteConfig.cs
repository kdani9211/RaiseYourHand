using System.Web.Mvc;
using System.Web.Routing;

namespace RaiseYourHand
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute
			(
				name: "Default",
				url: "{action}/{room}",
				defaults: new { controller = "Main", action = "Participant", room = UrlParameter.Optional }
			);
		}
	}
}
