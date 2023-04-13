    using AutoMapper;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;

namespace WebApiAutores.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public  AutoMapperProfiles()
        {
            CreateMap<AutorCreationDTO, Autor>();
            CreateMap<Autor, AutorDTO>()
             .ForMember(AutorDTO => AutorDTO.Books, options => options.MapFrom(MapBooksAuthors));

            CreateMap<BookCreationDTO, Book>()
                .ForMember(Book=>Book.AuthorsBooks, options=>options.MapFrom(MapAuthorsBooks));

            CreateMap<Book, BookDTO>()
            .ForMember(BookDTO => BookDTO.Autors, options => options.MapFrom(MapBookDTOAutors));

            CreateMap<BookPatchDTO, Book>().ReverseMap();

            CreateMap<CommentCreationDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
            CreateMap<Comment, CommentDTO>();
        }


        private List<BookDTO> MapBooksAuthors(Autor author, AutorDTO authorDTO)
        {
            var result = new List<BookDTO>();
            if (author.AuthorsBooks == null)
            {
                return result;
            }
            foreach (var authorBook in author.AuthorsBooks)
            {
                result.Add(new BookDTO()
                {
                    Id=authorBook.BookId,
                    Title= authorBook.Book.Title
                });
            }
            return result;
        }
            private List<AutorDTO> MapBookDTOAutors(Book book, BookDTO bookDTO)
        {
            var result = new List<AutorDTO>();
            if (book.AuthorsBooks == null)
            {
                return result;
            }
            foreach (var bookAutor in book.AuthorsBooks)
            {
                result.Add(new AutorDTO()
                {
                    Id = bookAutor.AuthorId,
                    Name= bookAutor.Autor.Name
                });
                
            }
            return result;

        }
            private List<AuthorBook> MapAuthorsBooks(BookCreationDTO bookCreationDTO, Book book)
        {
            var result=new List<AuthorBook>();

            if (bookCreationDTO.AuthorsIds == null)
            {
                return result;
            }

            foreach (var authorId in bookCreationDTO.AuthorsIds)
            {
                result.Add(new AuthorBook() { AuthorId = authorId });
            }
            return result;
        }
    }
}
