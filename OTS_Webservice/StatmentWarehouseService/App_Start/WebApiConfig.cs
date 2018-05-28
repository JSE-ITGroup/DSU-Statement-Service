using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace StatmentWarehouseService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
         
             config.Routes.MapHttpRoute(
                name: "GetAccountInfo",
                routeTemplate: "jse/{controller}/GetAccountInfo/{AccountNumber}",
                defaults: new          {
                                          AccountNumber = RouteParameter.Optional,
                                          Action = "GetAccountInfo"
                                       }
                                       );

             config.Routes.MapHttpRoute(
                 name: "GetCommittment",
                 routeTemplate: "jse/{controller}/GetCommittment/{AccountNumber}",
                 defaults: new
                                      {
                                          AccountNumber = RouteParameter.Optional,
                                          Action = "GetCommittment"
                                      }
                                      );


            config.Routes.MapHttpRoute(
                    name: "GetStatement",
                    routeTemplate: "jse/{controller}/GetStatement/{AccountNumber}/{StartDate}/{EndDate}",
                    defaults: new
                                          {
                                              AccountNumber = RouteParameter.Optional,
                                              StartDate = RouteParameter.Optional,
                                              EndDate = RouteParameter.Optional,
                                              Action = "GetStatement"
                                          }

                                          );

            config.Routes.MapHttpRoute(
                name: "GetPortfolio",
                routeTemplate: "jse/{controller}/GetPortfolio/{AccountNumber}/{StartDate}/{EndDate}/{Token}",
                defaults: new
                                         {
                                             AccountNumber = RouteParameter.Optional,
                                             StartDate = RouteParameter.Optional,
                                             EndDate = RouteParameter.Optional,
                                             Token = RouteParameter.Optional,
                                             Action = "GetPortfolio"
                                         }

                                         );

            config.Routes.MapHttpRoute(
                                        name: "PortfolioDownload",
                                        routeTemplate: "jse/{controller}/PortfolioDownload/{AccountNumber}/{StartDate}/{EndDate}/{Token}/{Token2}",
                                        defaults: new
                                        {
                                            AccountNumber = RouteParameter.Optional,
                                            StartDate = RouteParameter.Optional,
                                            EndDate = RouteParameter.Optional,
                                            Token = RouteParameter.Optional,
                                            Token2 = RouteParameter.Optional,
                                            Action = "PortfolioDownload"
                                        }

                                        );
            config.EnableSystemDiagnosticsTracing();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("json", "true", "application/json"));

        }
    }
}
