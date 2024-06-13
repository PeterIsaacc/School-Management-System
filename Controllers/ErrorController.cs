using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace SchoolManagementSystem.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Home/Error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error; 
            var code = HttpContext.Response.StatusCode; 
    
            var viewModel = new ErrorViewModel
            {
                StatusCode = code,
                Message = exception?.Message ?? "An unexpected error occurred."
            };
    
            return View("~/Views/Home/Error.cshtml", viewModel);
        }
    }
}