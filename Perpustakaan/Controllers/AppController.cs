using Microsoft.AspNetCore.Mvc;

namespace Perpustakaan.Controllers
{
    [Route("/")]
    public class AppController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Index()
        {
            return Ok("Welcome to Auth App");
        }
    }
}
