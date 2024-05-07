using Microsoft.AspNetCore.Mvc.Rendering;

namespace N4Core.Route.Utils
{
    public static class MvcRouteUtil
    {
        public static string GetCurrentRoute(ViewContext viewContext)
        {
            string currentRoute = $"/{viewContext.RouteData.Values["Controller"]}/{viewContext.RouteData.Values["Action"]}";
            if (viewContext.RouteData.Values["Area"] is not null)
                currentRoute = $"/{viewContext.RouteData.Values["Area"]}" + currentRoute;
            if (viewContext.RouteData.Values["id"] is not null)
                currentRoute += $"/{viewContext.RouteData.Values["id"]}";
            return currentRoute;
        }

        public static string GetHomeRoute() => $"/Home/Index";

        public static string GetHomeRoute(string area) => $"/{area}{GetHomeRoute()}";

        public static string GetHomeRoute(ViewContext viewContext)
        {
            string homeRoute = GetHomeRoute();
            if (viewContext.RouteData.Values["Area"] is not null)
                homeRoute = $"/{viewContext.RouteData.Values["Area"]}" + homeRoute;
            return homeRoute;
        }

        public static string GetReturnRoute(string returnUrl) => string.IsNullOrWhiteSpace(returnUrl) || returnUrl.Contains("//") ? 
            GetHomeRoute() : returnUrl;
    }
}
