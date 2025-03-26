using BookStore.API.Models;
using BookStore.API.QueryParameters;
using BookStore.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BookStore.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController(IBookService bookService) : ControllerBase
    {
        private readonly IBookService _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] BookFilterOptions filterOptions)
        {
            var result = await _bookService.GetBooksAsync(filterOptions);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(result.Pagination));

            return Ok(result.Items);
        }

        [HttpGet("{bookId}")]
        public async Task<ActionResult<BookDto>> GetBookDetailsById(int bookId)
        {
            try
            {
                var result = await _bookService.GetBookDetailsByIdAsync(bookId);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook(BookForCreationDto bookDto)
        {
            var createdBook = await _bookService.CreateBookAsync(bookDto);

            return CreatedAtAction(nameof(GetBookDetailsById), new
            {
                bookId = createdBook.Id
            }, createdBook);
        }
    }
}
