using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;
using WebApiAutores.Migrations;
using static System.Reflection.Metadata.BlobBuilder;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/Books")]
    public class BooksController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            this.context= context;
            this.mapper = mapper;
        }
        [HttpGet( Name = "getBooks")]
        public async Task<ActionResult<List<BookDTO>>> Get()
        {
           var books= await context.Books.ToListAsync();
            return mapper.Map<List<BookDTO>>(books);   
        }
        [HttpGet("{id:int}", Name ="getBook")]
        public async Task<ActionResult<BookDTO>> Get(int id)
        {
            var book= await context.Books
                .Include(bookDB => bookDB.AuthorsBooks)
                .ThenInclude(AuthorsBookDB=> AuthorsBookDB.Autor)
               // .Include(bookDB=>bookDB.Comments)
                
                .FirstOrDefaultAsync(x => x.Id == id); 
         return  mapper.Map<BookDTO>(book);  
        }
        [HttpPost(Name = "createBook")]
        public async Task<ActionResult<BookDTO>> Post(BookCreationDTO bookCreationDTO)
        {
            if (bookCreationDTO.AuthorsIds==null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }
            var authorsIds = await context.Autors
                .Where(authorDB => bookCreationDTO.AuthorsIds.Contains(authorDB.Id))
                .Select(x => x.Id).ToListAsync();
            if(bookCreationDTO.AuthorsIds.Count!= authorsIds.Count)
            {
                return BadRequest("Uno de los autores enviados no existe");
            }
            var book = mapper.Map<Book>(bookCreationDTO);
            context.Add(book);
            await context.SaveChangesAsync();
            var bookDTO = mapper.Map<BookDTO>(book);
            return CreatedAtRoute("getBook", new { id = book.Id }, bookDTO);
        }

        [HttpPut("{id:int}", Name = "editBook")] // api/autores/1 
        public async Task<ActionResult> Put(BookCreationDTO autorCreacionDTO, int id)
        {
            var existe = await context.Autors.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("{id:int}",Name = "updateBook")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<BookPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var libroDB = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (libroDB == null)
            {
                return NotFound();
            }

            var libroDTO = mapper.Map<BookPatchDTO>(libroDB);

            patchDocument.ApplyTo(libroDTO, ModelState);

            var esValido = TryValidateModel(libroDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(libroDTO, libroDB);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "deleteBook")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Books.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Book() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
