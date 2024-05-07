using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using System.Threading;

namespace Broker.Helpers
{
    /*[AttributeUsage(AttributeTargets.Method)]
     public class LanguageAttribute : Attribute
     {
         private static HttpContext _context { get; set; }
         public LanguageAttribute(string key = "LanguageCode")
         {
             var language = _context.Request.Headers[key];
             if (!string.IsNullOrEmpty(language))
             {
                 CultureInfo ci;
                 switch (language)
                 {
                     case "ar":
                         ci = new CultureInfo("ar-Eg");
                         break;
                     case "en":
                         ci = new CultureInfo("en");
                         break;
                     default:
                         ci = new CultureInfo("ar-Eg");
                         break;
                 }
                 ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                 ci.DateTimeFormat.LongTimePattern = "hh:mm:ss";

                 Thread.CurrentThread.CurrentUICulture = ci;
                 CultureInfo.DefaultThreadCurrentUICulture = ci;

                 Thread.CurrentThread.CurrentCulture = ci;
                 CultureInfo.DefaultThreadCurrentCulture = ci;
             }

         }

     }*/


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class LanguageAttribute : ActionFilterAttribute
    {
        private readonly string _featureName;
        public LanguageAttribute(string featureName)
        {
            _featureName = featureName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var language = context.HttpContext.Request.Headers[_featureName];
            if (!string.IsNullOrEmpty(language))
            {
                CultureInfo ci;
                switch (language)
                {
                    case "ar":
                        ci = new CultureInfo("ar-Eg");
                        break;
                    case "en":
                        ci = new CultureInfo("en");
                        break;
                    default:
                        ci = new CultureInfo("ar-Eg");
                        break;
                }
                ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                ci.DateTimeFormat.LongTimePattern = "hh:mm:ss";

                Thread.CurrentThread.CurrentUICulture = ci;
                CultureInfo.DefaultThreadCurrentUICulture = ci;

                Thread.CurrentThread.CurrentCulture = ci;
                CultureInfo.DefaultThreadCurrentCulture = ci;
            }
        }
    }

}
