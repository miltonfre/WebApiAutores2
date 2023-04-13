using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiAutores.DTOs;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]
    public class RouteController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RouteController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }
        [HttpGet(Name = "getRoute")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DatoHATEOAS>>> GetRoute()
        {
            var datosHateos=new List<DatoHATEOAS>();

            var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");
            datosHateos.Add(
                new DatoHATEOAS(enlace: Url.Link("getRoute", new { }),
                    descripcion: "self", metodo: "GET")
                );
            datosHateos.Add(
                new DatoHATEOAS(enlace: Url.Link("getAuthors", new { }),
                    descripcion: "authors", metodo: "GET")
                );

            if (isAdmin.Succeeded)
            {
                datosHateos.Add(
               new DatoHATEOAS(enlace: Url.Link("createAuthor", new { }),
                   descripcion: "author-create", metodo: "POST")
               );
                datosHateos.Add(
              new DatoHATEOAS(enlace: Url.Link("createBook", new { }),
                  descripcion: "book-create", metodo: "POST")
              );
            }
           
            return datosHateos;

        }
    }
}
