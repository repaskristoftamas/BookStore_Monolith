using AutoMapper;
using BookStore.API.Models;
using BookStore.API.QueryParameters;
using BookStore.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BookStore.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController(IAuthorService authorService, IMapper mapper) : ControllerBase
    {
        private readonly IAuthorService _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors([FromQuery] AuthorQueryParameters authorQueryParameters)
        {
            var result = await _authorService.GetAuthors(authorQueryParameters);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(result.Pagination));

            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(result.Items));
        }

        [HttpGet("by-book/{bookId}")]
        public async Task<ActionResult<AuthorDto>> GetAuthorByBookId(int bookId)
        {
            try
            {
                var result = await _authorService.GetAuthorByBookIdAsync(bookId);

                return Ok(_mapper.Map<AuthorDto>(result));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
