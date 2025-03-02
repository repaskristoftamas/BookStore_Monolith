using AutoMapper;
using BookStore.API.Models;
using BookStore.API.QueryParameters;
using BookStore.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BookStore.API.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenreController(IGenreService genreService, IMapper mapper) : ControllerBase
    {
        private readonly IGenreService _genreService = genreService ?? throw new ArgumentNullException(nameof(genreService));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres([FromQuery] GenreQueryParameters genreQueryParameters)
        {
            var result = await _genreService.GetGenres(genreQueryParameters);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(result.Pagination));

            return Ok(_mapper.Map<IEnumerable<GenreDto>>(result.Items));
        }
    }
}
