using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.ValidationAttributes
{
    public class EnsurePositiveNumberAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var limit = Convert.ToInt32(context.ActionArguments["limit"]);
            if (limit < 1)
            {
                context.ModelState.AddModelError("limit", "Limit value must be positive");
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
