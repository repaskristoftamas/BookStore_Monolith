using AutoMapper;
using BookStore.API.Entities;
using BookStore.API.Models;

namespace BookStore.API.AutoMapperProfiles
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, GenreDto>();
            CreateMap<GenreDto, Genre>();
            CreateMap<GenreForCreationDto, Genre>();
        }
    }
}
