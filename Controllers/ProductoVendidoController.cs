using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionWebApi_EnzoDonadel.Models;
using SistemaGestionWebApi_EnzoDonadel.Repository;

namespace SistemaGestionWebApi_EnzoDonadel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoVendidoController : ControllerBase
    {
        [HttpGet("{idUsuario}")]
        public List<ProductoVendido> TraerProductoVendido(long idUsuario)
        {
            return ProductoVendidoHandler.GetProductoVendidoByUserName(idUsuario);
        }
    }
}
