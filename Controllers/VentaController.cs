using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionWebApi_EnzoDonadel.Models;
using SistemaGestionWebApi_EnzoDonadel.Repository;

namespace SistemaGestionWebApi_EnzoDonadel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        [HttpPost("{idUsuario}")]
        public void CrearVenta(long idUsuario, List<Producto> DataToAdd)
        {
            VentaHandler.CrearVenta(idUsuario, DataToAdd);
        }
    }
}
