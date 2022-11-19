using System.Web;
using System.Web.Mvc;

namespace Cumulative_1_assignment_3
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
