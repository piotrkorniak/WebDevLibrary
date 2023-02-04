using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LibraryApi.Command;
using LibraryApi.Controllers.Abstract;
using LibraryApi.DTO;
using LibraryApi.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : BaseApiController
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("rental")]
        public async Task<ActionResult<IEnumerable<RentalResponse>>> GetAllRentals(CancellationToken cancellationToken)
        {
            var getAllRentalsQuery = new GetAllRentalsQuery();
            var allRentals = await _mediator.Send(getAllRentalsQuery, cancellationToken);
            return Ok(allRentals);
        }


        [HttpGet("rental/{id:int}")]
        public async Task<ActionResult<RentalResponse>> GetRental([FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var getRentalQuery = new GetRentalQuery
            {
                RentalId = id
            };
            var rental = await _mediator.Send(getRentalQuery, cancellationToken);
            return rental;
        }

        [HttpPatch("rental/{id:int}/active")]
        public async Task<ActionResult> ActiveRental([FromRoute] int id, CancellationToken cancellationToken)
        {
            var activeRentalCommand = new ActiveRentalCommand
            {
                RentalId = id
            };
            await _mediator.Send(activeRentalCommand, cancellationToken);
            return Ok();
        }

        [HttpPatch("rental/{id:int}/close")]
        public async Task<ActionResult> CloseRental([FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var closeRentalCommand = new CloseRentalCommand
            {
                RentalId = id
            };
            await _mediator.Send(closeRentalCommand, cancellationToken);
            return Ok();
        }

        [HttpDelete("book/{id:int}")]
        public async Task<ActionResult> DeleteBook([FromRoute] int id, CancellationToken cancellationToken)
        {
            var deleteBookCommand = new DeleteBookCommand
            {
                Id = id
            };
            await _mediator.Send(deleteBookCommand, cancellationToken);
            return Ok();
        }

        [HttpPost("book")]
        public async Task<ActionResult> AddBook([FromBody] AddBookCommand addBookCommand,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(addBookCommand, cancellationToken);
            return Ok();
        }
    }
}