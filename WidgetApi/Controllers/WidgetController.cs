using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WidgetApi.Models;

namespace WidgetApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WidgetController : ControllerBase
    {
        [HttpGet]
       // [Authorize("dataEventRecordsUser")]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(100); // simulate latency
            var items = new List<Widget>()
            {
                new Widget { ID = 1, Name = "Cog", Shape = "Square" },
                new Widget { ID = 2, Name = "Gear", Shape = "Round" },
                new Widget { ID = 3, Name = "Sprocket", Shape = "Octagonal" },
                new Widget { ID = 4, Name = "Pinion", Shape = "Triangular" }
            };

            return Ok(items);
        }

      //  [Authorize("dataEventRecordsAdmin")]
        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Get(string input)
        {
            await Task.Delay(100); // simulate latency
            
            return Ok("called by admin:{input}");
        }
    }
}
