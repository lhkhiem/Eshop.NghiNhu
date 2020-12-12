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
                name: "ProductCategory",
                url: "admin/danh-muc-san-pham",
                defaults: new { controller = "ProductCategory", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                 name: "ProductCategory-Create",
                 url: "admin/them-danh-muc-san-pham",
                 defaults: new { controller = "ProductCategory", action = "Create", id = UrlParameter.Optional }
             );
            context.MapRoute(
                 name: "ProductCategory-Edit",
                 url: "admin/cap-nhat-danh-muc-san-pham/{id}",
                 defaults: new { controller = "ProductCategory", action = "Edit", id = UrlParameter.Optional }
             );
            context.MapRoute(
                name: "Unit",
                url: "admin/don-vi",
                defaults: new { controller = "Unit", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                name: "Unit-Create",
                url: "admin/them-don-vi",
                defaults: new { controller = "Unit", action = "Create", id = UrlParameter.Optional }
            );
            context.MapRoute(
                name: "Unit-Update",
                url: "admin/cap-nhat-don-vi/{id}",
                defaults: new { controller = "Unit", action = "Edit", id = UrlParameter.Optional }
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