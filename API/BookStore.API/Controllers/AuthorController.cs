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
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors([FromQuery] AuthorFilterOptions authorQueryParameters)
        {
            var result = await _authorService.GetAuthors(authorQueryParameters);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(result.Pagination));

            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(result.Items));
        }

        [HttpGet("{authorId}")]
        public async Task<ActionResult<AuthorDto>> GetAuthorDetailsById(int authorId)
        {
            try
            {
                var result = await _authorService.GetAuthorDetailsById(authorId);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(AuthorForCreationDto authorDto)
        {
            var createdAuthor = await _authorService.CreateAuthorAsync(authorDto);

            return CreatedAtAction(nameof(GetAuthorDetailsById), new
            {
                authorId = createdAuthor.Id,
            }, createdAuthor);
        }
    }
}
