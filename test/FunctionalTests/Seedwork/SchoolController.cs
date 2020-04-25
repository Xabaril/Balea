using System.Linq;
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
            var authenticatedSchemes = User.Identities
                .Where(i => i.IsAuthenticated)
                .Select(i => i.AuthenticationType);

            return Ok(authenticatedSchemes);
        }

        [HttpGet]
        [Route("grades")]
        [Authorize(Policy = Policies.ViewGrades)]
        public IActionResult GetGrades()
        {
            var authenticatedSchemes = User.Identities
                .Where(i => i.IsAuthenticated)
                .Select(i => i.AuthenticationType);

            return Ok(authenticatedSchemes);
        }

        [HttpGet]
        [Route("schemes")]
        [Authorize] // This apply default authentication scheme
        public IActionResult GetSchemes()
        {
            var authenticatedSchemes = User.Identities
                .Where(i => i.IsAuthenticated)
                .Select(i => i.AuthenticationType);

            return Ok(authenticatedSchemes);
        }

        [HttpGet]
        [Route("custom-policy")]
        [Authorize(Policies.Custom)] 
        public IActionResult GetCustomPolicy()
        {
            var authenticatedSchemes = User.Identities
                .Where(i => i.IsAuthenticated)
                .Select(i => i.AuthenticationType);

            return Ok(authenticatedSchemes);
        }
    }
}
