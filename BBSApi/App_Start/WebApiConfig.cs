using System.Web.Http;

namespace BBSApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{DomainId}", 
                new {id = RouteParameter.Optional}
                );
        }
    }
}