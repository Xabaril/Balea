using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunctionalTests.Seedwork
{
    [Route("api/school")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        [HttpPut]
        [Route("grades")]
        [Authorize(Policy = Policies.EditGrades)]
        public IActionResult EditGrades()
        {
            return Ok();
        }

        [HttpGet]
        [Route("grades")]
        [Authorize(Policy = Policies.ViewGrades)]
        public IActionResult GetGrades()
        {
            return Ok();
        }
    }
}
