using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EC.Website.Filters
{
    public class UserFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool IsAdmin = AuthUser.LoggedUser.IsAdmin;
            if (IsAdmin)
            {
                context.HttpContext.Response.Redirect("/Home/Index");
                context.Result = new EmptyResult();
            }
            base.OnActionExecuting(context);
        }
    }
}
