using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(Api.Startup))]

namespace Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 如需如何設定應用程式的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkID=316888
            //app.UseCors(CorsOptions.AllowAll); // You can modify the CorsOptions

            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll); // You can modify the CorsOptions
                map.RunSignalR(new HubConfiguration { EnableJSONP = true });
            });
        }
    }
}