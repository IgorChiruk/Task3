using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Web_Chat.Startup))]

namespace Web_Chat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            // Дополнительные сведения о настройке приложения см. на странице https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
