using AutoMapper;
using BookStore.API.Entities;
using BookStore.API.Helpers;
using BookStore.API.Models;

namespace BookStore.API.AutoMapperProfiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<BookForCreationDto, Book>();
            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>))
            .ConvertUsing(typeof(PagedResultConverter<,>));
        }
    }
}
