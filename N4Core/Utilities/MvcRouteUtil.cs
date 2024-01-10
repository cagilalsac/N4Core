using Microsoft.AspNetCore.Mvc.Rendering;

namespace N4Core.Utilities
{
    public static class MvcRouteUtil
    {
        public static string GetCurrentRoute(ViewContext viewContext)
        {
            string currentRoute = $"/{viewContext.RouteData.Values["Controller"]}/{viewContext.RouteData.Values["Action"]}";
            if (viewContext.RouteData.Values["Area"] is not null)
                currentRoute = $"/{viewContext.RouteData.Values["Area"]}" + currentRoute;
            return currentRoute;
        }

        public static string GetHomeRoute(ViewContext viewContext)
        {
            string homeRoute = "/Home/Index";
            if (viewContext.RouteData.Values["Area"] is not null)
                homeRoute = $"/{viewContext.RouteData.Values["Area"]}" + homeRoute;
            return homeRoute;
        }

        public static string GetHomeRoute(string area) => $"/{area}/Home/Index";

        public static string GetHomeRoute() => $"/Home/Index";
    }
}
