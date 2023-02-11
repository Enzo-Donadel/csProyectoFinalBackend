using Microsoft.AspNetCore.Mvc;
using SistemaGestionWebApi_EnzoDonadel.Models;
using SistemaGestionWebApi_EnzoDonadel.Repository;

namespace SistemaGestionWebApi_EnzoDonadel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        //LogIn de Usuario.
        [HttpGet("{userName}/{password}")]
        public Usuario LogIn(string userName, string password)
        {
            Usuario user = UsuarioHandler.UserLogIn(userName, password);
            return user;
        }

        //Modifica Datos de Usuario
        [HttpPut]
        public void ModificarUsuario(Usuario DataToModify)
        {
            UsuarioHandler.UpdateUsuario(DataToModify);
        }
    }
}
