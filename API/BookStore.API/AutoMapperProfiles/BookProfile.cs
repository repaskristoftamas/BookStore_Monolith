using AutoMapper;
using BookStore.API.Entities;
using BookStore.API.Models;

namespace BookStore.API.AutoMapperProfiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>();
        }
    }
}
