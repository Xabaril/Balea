using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Balea.UI.Api.UseCases.Applications
{
    [ApiController]
    [Route("api/applications")]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ApplicationsController(IMediator mediator)
        {
            Ensure.Argument.NotNull(mediator, nameof(mediator));
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Show()
        {
            return Ok();
        }
    }
}
