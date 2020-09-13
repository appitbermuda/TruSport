using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
//using Microsoft.Owin;
//using Microsoft.Owin.Security.OAuth;
using OnTrackWebService.Data;
//using Owin;

//[assembly: OwinStartup(typeof(OnTrackWebService.Startup))]

namespace OnTrackWebService
{
    public class OwinStartup
    {
        //public void Configuration(IAppBuilder app)
        //{
        //    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        //    app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

        //    OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions()
        //    {
        //        AllowInsecureHttp = true,

        //        TokenEndpointPath = new PathString("/token"),

        //        AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),

        //        Provider = new AuthorizationServerProvider()
        //    };

        //    app.UseOAuthAuthorizationServer(options);

        //    app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        //    //HttpConfiguration config = new HttpConfiguration();
        //    //ApiConfig.Register(config);
        //}
    }
}
