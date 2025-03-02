﻿namespace BookStore.API.Models
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public AuthorDto? Author { get; set; }
        public GenreDto? Genre { get; set; }
    }
}
