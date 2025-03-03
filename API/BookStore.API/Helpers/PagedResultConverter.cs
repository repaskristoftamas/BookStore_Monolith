using AutoMapper;
using BookStore.API.Models;

namespace BookStore.API.Helpers
{
    public class PagedResultConverter<TSource, TDestination>(IMapper mapper) : ITypeConverter<PagedResult<TSource>, PagedResult<TDestination>>
    {
        private readonly IMapper _mapper = mapper;

        public PagedResult<TDestination> Convert(PagedResult<TSource> source, PagedResult<TDestination> destination, ResolutionContext context)
        {
            var mappedItems = _mapper.Map<List<TDestination>>(source.Items);

            return new PagedResult<TDestination>(mappedItems, source.Pagination);
        }
    }

}
