using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LibraryApi.Controllers.Abstract;
using LibraryApi.DTO;
using LibraryApi.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    public class BookController : BaseApiController
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBooks(
            [FromQuery] string globalSearch,
            [FromQuery] bool status,
            CancellationToken cancellationToken)
        {
            var getBooksQuery = new GetBooksQuery
            {
                SearchValue = globalSearch,
                IsAvailable = status
            };
            var books = await _mediator.Send(getBooksQuery, cancellationToken);
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookResponse>> GetBook([FromRoute] int id, CancellationToken cancellationToken)
        {
            var getBookQuery = new GetBookQuery
            {
                BookId = id
            };
            var book = await _mediator.Send(getBookQuery, cancellationToken);
            return book;
        }
    }
}