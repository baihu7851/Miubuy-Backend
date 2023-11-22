using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //加入驗證
            //GlobalConfiguration.Configuration.Filters.Add(new JwtAuth());
        }
    }
}