using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.ModelBinding;
using System.Web.Routing;
using Serilog;
using WebApi.App_Start;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //SerilogConfig.Setup();
            //ModelValidatorProviders.Providers.Remove(ModelValidatorProviders.Providers.OfType<DataAnnotationsModelValidatorProvider>().Single());
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
        protected void Application_End()
        {
            //Log.CloseAndFlush();
        }
    }
}
