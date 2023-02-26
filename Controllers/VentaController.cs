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
        public bool CrearVenta(long idUsuario, List<Producto> DataToAdd)
        {
            if (!VentaHandler.CrearVenta(idUsuario, DataToAdd))
            {
                throw new HttpRequestException("La Venta no se a realizado correctamente.");
            }
            else return true;
        }
        [HttpGet("{idUsuario}")]
        public List<Venta> TraerVentas(long idUsuario)
        {
            return VentaHandler.GetVentaByUserId(idUsuario);
        }
    }
}
