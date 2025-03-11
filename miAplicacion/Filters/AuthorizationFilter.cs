using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace miAplicacion.Filters
{
    public class AuthorizationFilter : IPageFilter
    {
        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var page = context.HandlerInstance as PageModel;
            if (page != null && context.HttpContext.User.Identity != null && !context.HttpContext.User.Identity.IsAuthenticated)
            {
                // Si no es la página de login y el usuario no está autenticado
                if (!context.HttpContext.Request.Path.StartsWithSegments("/Login"))
                {
                    context.Result = new RedirectToPageResult("/Login");
                }
            }
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context) { }
        public void OnPageHandlerSelected(PageHandlerSelectedContext context) { }
    }
} 