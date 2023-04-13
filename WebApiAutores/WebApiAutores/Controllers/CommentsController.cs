using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/books/{BookId:int}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public CommentsController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        [HttpGet("{commentId:int}", Name = "getComment")]
        public async Task<ActionResult<CommentDTO>> GetById(int commentId) {

           var comment=await context.Comments.FirstOrDefaultAsync(commentDB=>commentDB.Id== commentId);
            if (comment==null)
            {
                return NotFound();
            }
           var commentDTO= mapper.Map<CommentDTO>(comment);    
            return Ok(commentDTO);
        }

        [HttpGet(Name = "getComments")]
        public async Task<ActionResult<List<CommentDTO>>> Get(int BookId) {

            if (!await context.Books.AnyAsync(x => x.Id == BookId))
            {
                return NotFound("Book doesn't exist");
            }
            var comments= await context.Comments.Where(x=>x.BookId== BookId).ToListAsync();

            return mapper.Map<List<CommentDTO>>(comments) ;
        
        }
        [HttpPost(Name = "createComment")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int BookId, CommentCreationDTO commentCreationDTO)
        {           
            if (!await context.Books.AnyAsync(x => x.Id == BookId))
            {
                return NotFound("Book doesn't exist");
            }
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var user=await userManager.FindByEmailAsync(email);
            var userId=user.Id;
            var comment = mapper.Map<Comment>(commentCreationDTO);
            comment.BookId = BookId;
            comment.UserId= userId;
            context.Add(comment);
            await context.SaveChangesAsync();
            var commentDTO=mapper.Map<CommentDTO>(comment);
            return CreatedAtRoute("getComment",new {id= comment .Id, BookId =comment.BookId}, commentDTO);
        }

        [HttpPut("{commentId:int}", Name = "updateComment")]
        public async Task<ActionResult> Put(int BookId,int commentId, CommentCreationDTO commetCreationDTO)
        {
            if (!await context.Books.AnyAsync(x => x.Id == BookId))
            {
                return NotFound("Book doesn't exist");
            }

            if (!await context.Comments.AnyAsync(x => x.Id == commentId))
            {
                return NotFound("Comment doesn't exist");
            }
            var comment = mapper.Map<Comment>(commetCreationDTO);
            comment.Id = commentId;
            comment.BookId = BookId;
            context.Update(comment);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
