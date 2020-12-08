using System.Web.Mvc;

namespace EShop.Areas.Admin
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
                name: "Unit",
                url: "admin/don-vi",
                defaults: new { controller = "Unit", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                name: "Login",
                url: "admin/",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                name: "Screen",
                url: "admin/man-hinh-chinh",
                defaults: new { controller = "Screen", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}