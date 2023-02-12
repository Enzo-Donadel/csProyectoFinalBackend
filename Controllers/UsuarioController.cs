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
        [HttpGet("{usuario}/{contraseña}")]
        public Usuario LogIn(string usuario, string contraseña)
        {
            Usuario user = UsuarioHandler.UserLogIn(usuario, contraseña);
            return user;
        }

        //Modifica Datos de Usuario
        [HttpPut]
        public void ModificarUsuario(Usuario DataToModify)
        {
            UsuarioHandler.UpdateUsuario(DataToModify);
        }
        [HttpPost]
        public void CrearUsuario(Usuario usuario)
        {
            if (!UsuarioHandler.InsertUsuario(usuario))
            {
                throw new HttpRequestException("El nombre de usuario yá esta en uso. Elija Otro.");
            }
        }
        [HttpGet("{usuario}")]
        public Usuario TraerUsuario(string usuario)
        {
            return UsuarioHandler.GetUsuarioByUserName(usuario);
        }
        [HttpDelete("{id}")]
        public void BorrarUsuario(long id)
        {
            UsuarioHandler.DeleteUser(id);
        }

    }
}
