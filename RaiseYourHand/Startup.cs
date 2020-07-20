using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(RaiseYourHand.Startup))]
namespace RaiseYourHand
{
	public class Startup
	{
		public void Configuration(IAppBuilder app) =>
			app.MapSignalR();
	}
}