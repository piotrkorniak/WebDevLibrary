using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LibraryApi.Command;
using LibraryApi.Controllers.Abstract;
using LibraryApi.DTO;
using LibraryApi.ExtensionMethods;
using LibraryApi.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("rental")]
        public async Task<ActionResult<IEnumerable<RentalResponse>>> GetUserRentals(CancellationToken cancellationToken)
        {
            var getUserRentalsQuery = new GetUserRentalsQuery
            {
                UserId = GetCurrentUserId()
            };
            var rentals = await _mediator.Send(getUserRentalsQuery, cancellationToken);
            return Ok(rentals);
        }


        [HttpGet("rental/{id:int}")]
        public async Task<ActionResult<RentalResponse>> GetUserRental([FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var getRentalQuery = new GetUserRentalQuery
            {
                RentalId = id,
                UserId = GetCurrentUserId()
            };
            var rental = await _mediator.Send(getRentalQuery, cancellationToken);
            return rental;
        }

        [HttpPost("rental")]
        public async Task<ActionResult> AddUserRental([FromBody] AddUserRentalCommand addUserRentalCommand,
            CancellationToken cancellationToken)
        {
            addUserRentalCommand.Bind(x => x.UserId, GetCurrentUserId());
            await _mediator.Send(addUserRentalCommand, cancellationToken);
            return Ok();
        }

        [HttpPatch("rental/{id:int}/close")]
        public async Task<ActionResult> ActiveUserRental([FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var closeUserRentalCommand = new CloseUserRentalCommand
            {
                UserId = GetCurrentUserId(),
                RentalId = id
            };
            await _mediator.Send(closeUserRentalCommand, cancellationToken);
            return Ok();
        }
    }
}