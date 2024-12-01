using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
namespace DATN_QLTiemChung
{
    public class SessionActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;

            // Lấy thông tin từ session
            string userId = httpContext.Session.GetString("ID");
            string username = httpContext.Session.GetString("Username");
            string userRole = httpContext.Session.GetString("Role");

            // Lưu thông tin vào TempData
            httpContext.Items["ID"] = userId;
            httpContext.Items["Username"] = username;
            httpContext.Items["Role"] = userRole;

            base.OnActionExecuting(context);
        }
    }
}
