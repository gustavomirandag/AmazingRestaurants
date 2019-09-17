using System.Web;
using System.Web.Mvc;

namespace AmazingRestaurants.IamMicroservice
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
