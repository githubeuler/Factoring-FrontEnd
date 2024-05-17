using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Factoring.WebMvc.Helpers
{
    public static class ApplicationHelpers
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static string IsActive(this IHtmlHelper html, string controller = null, string action = null)
        {
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            if (String.IsNullOrEmpty(controller))
            {
                controller = currentController;
            }
            if (String.IsNullOrEmpty(action))
            {
                action = currentAction;
            }
            return controller.ToLower().Split(',').Contains(currentController.ToLower()) && action.ToLower().Split(',').Contains(currentAction.ToLower()) ? "active" : String.Empty;
        }

        public static string IsCollapse(this IHtmlHelper html, string controller = null)
        {
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];
            return controller.ToLower().Split(',').Contains(currentController.ToLower()) ? "here show" : String.Empty;
        }

        public static string GetContentType(string filename)
        {
            string extension = filename.Split(".")[1];
            switch (extension)
            {
                case "pdf":
                    return "application/pdf";

                case "png":
                    return "image/png";

                case "xml":
                    return "text/xml";

                case "jpg":
                    return "image/jpg";

                case "docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

                default:
                    return "error";
            }
        }
    }
}
