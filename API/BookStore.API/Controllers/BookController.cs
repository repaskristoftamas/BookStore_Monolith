using AutoMapper;
using BookStore.API.Models;
using BookStore.API.QueryParameters;
using BookStore.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BookStore.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController(IBookService bookService, IMapper mapper) : ControllerBase
    {
        private readonly IBookService _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] BookQueryParameters bookQueryParameters)
        {
            var result = await _bookService.GetBooks(bookQueryParameters);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(result.Pagination));

            return Ok(_mapper.Map<IEnumerable<BookDto>>(result.Items));
        }
    }
}
