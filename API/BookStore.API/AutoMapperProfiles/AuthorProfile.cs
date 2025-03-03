using AutoMapper;
using BookStore.API.Entities;
using BookStore.API.Models;

namespace BookStore.API.AutoMapperProfiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<AuthorDto, Author>();
        }
    }
}
