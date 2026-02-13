using ApplicationTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        [HttpPost]
        public IActionResult Create(ApplicationModel application)
        {
            return Ok(application);
        }
    }
}
