using FriscoDev.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FriscoDev.UI.Attribute
{
    public class CheckLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookieId = LoginHelper.UserID;
            var cookieUserName = LoginHelper.UserName;
            if (string.IsNullOrEmpty(cookieId) || string.IsNullOrEmpty(cookieUserName))
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

    }
}