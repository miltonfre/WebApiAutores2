using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Policy ="isAdmin")]
    public class AutoresController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        
        [HttpGet( Name = "getAuthors")]               // api/autores
        [HttpGet("listado")]    // api/autores/listado
        [HttpGet("/listado")]   // listado
        [AllowAnonymous]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
          var autors= await context.Autors.ToListAsync();
            return mapper.Map<List<AutorDTO>>(autors);
        }

        [HttpGet("{id:int}", Name ="getAuthor")]
        public async Task<ActionResult<AutorDTO>> Get(int id)
        {
            var autor= await context.Autors
                .Include(authorBd=>authorBd.AuthorsBooks)
                .ThenInclude(authorBookDB=> authorBookDB.Book)
                .FirstOrDefaultAsync(autorBD => autorBD.Id == id);
            if (autor==null)
            {
                return NotFound();
            }
            return mapper.Map<AutorDTO>(autor);
        }


        [HttpGet("{name}", Name = "getAuthorByName")]
        public async Task<ActionResult<List<AutorDTO>>> Get(string name)
        {
            var autors = await context.Autors
                .Where(autorDB => autorDB.Name.Contains(name)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autors);
        }

        [HttpGet("primero", Name = "primero")]//api/autores/primero
        public async Task<ActionResult<Autor>> FirstAutor()
        {
            return await context.Autors.FirstOrDefaultAsync();
        }
        [HttpPost( Name = "createAuthor")]
        public async Task<ActionResult> Post(AutorCreationDTO autorCreationDTO)
        {
            var autorExist= await context.Autors.AnyAsync(x => x.Name == autorCreationDTO.Name);
            if (autorExist)
            {
                return BadRequest($"ya existe un autor con el nombre {autorCreationDTO.Name}");
            }
            var autor=mapper.Map<Autor>(autorCreationDTO);
            context.Add(autor);
           await context.SaveChangesAsync ();

            var autorDTO = mapper.Map<AutorDTO>(autor);
            return CreatedAtRoute("getAuthor", new { id= autorDTO.Id }, autorDTO);
        }

        [HttpPut("{id:int}", Name = "updateAuthor")]//api/autores/id
        public async Task<ActionResult> Put(int id,AutorCreationDTO autorCreationDTO)
        {
            var exist = await context.Autors.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }
            var author = mapper.Map<Autor>(autorCreationDTO);
            context.Update(author);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "deleteAuthor")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Autors.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }
            context.Remove(new Autor() { Id=id});
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
