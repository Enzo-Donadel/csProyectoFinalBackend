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

        //Modifica Datos de Usuario, regresa un valor verdadero en caso de haber afectado correctamente la base de datos.
        [HttpPut]
        public bool ModificarUsuario(Usuario DataToModify)
        {
            if (!UsuarioHandler.UpdateUsuario(DataToModify))
            {
                throw new HttpRequestException("El Usuario no ha podido ser modificado correctamente.");
            }
            else return true;
        }
        [HttpPost]
        public bool CrearUsuario(Usuario usuario)
        {
            if (!UsuarioHandler.InsertUsuario(usuario))
            {
                throw new HttpRequestException("El nombre de usuario yá esta en uso. Elija Otro.");
            }
            else return true;
        }
        [HttpGet("{usuario}")]
        public Usuario TraerUsuario(string usuario)
        {
            return UsuarioHandler.GetUsuarioByUserName(usuario);
        }
        [HttpDelete("{id}")]
        public bool BorrarUsuario(long id)
        {
            if(!UsuarioHandler.DeleteUser(id))
            {
                throw new HttpRequestException("El Usuario no ha podido ser borrado correctamente.");
            }
            else return true;
        }

    }
}
