using System.Web.Mvc;

namespace _23DH114665_MyStore.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
    "Admin_default",
    "Admin/{controller}/{action}/{id}",
    new { action = "Index", id = UrlParameter.Optional },
    new[] { "_23DH114665_MyStore.Areas.Admin.Controllers" }
);
        }
    }
}