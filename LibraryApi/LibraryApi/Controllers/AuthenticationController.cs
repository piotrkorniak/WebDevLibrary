using System.Threading;
using System.Threading.Tasks;
using LibraryApi.Command;
using LibraryApi.Controllers.Abstract;
using LibraryApi.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    public class AuthenticationController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginCommand command,
            CancellationToken cancellationToken)
        {
            var authenticationResponse = await _mediator.Send(command, cancellationToken);
            return authenticationResponse;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] RegisterCommand command,
            CancellationToken cancellationToken)
        {
            var authenticationResponse = await _mediator.Send(command, cancellationToken);
            return authenticationResponse;
        }
    }
}