using System.Web.Mvc;

namespace Maksimalist.Areas.mmadmin
{
    public class mmadminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "mmadmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "mmadmin_default",
                "mm-admin/{controller}/{action}/{id}",
                new { Controller ="Login" , action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}