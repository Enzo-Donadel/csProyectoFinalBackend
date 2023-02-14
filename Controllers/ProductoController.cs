using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionWebApi_EnzoDonadel.Models;
using SistemaGestionWebApi_EnzoDonadel.Repository;

namespace SistemaGestionWebApi_EnzoDonadel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        //Crea nuevoProducto
        [HttpPost]
        public bool CrearProducto(Producto DataToAdd)
        {
            if (!ProductoHandler.AddProducto(DataToAdd))
            {
                throw new HttpRequestException("El Producto no ha podido ser creado correctamente.");
            }
            else return true;
        }
        [HttpPut]
        public bool ModificarProducto(Producto DataToUpdate)
        {
            if (!ProductoHandler.UpdateProducto(DataToUpdate))
            {
                throw new HttpRequestException("El Producto no ha podido ser modificado correctamente.");
            }
            else return true;
        }
        [HttpDelete("{id}")]
        public bool BorrarProducto(long id)
        {
            if (!ProductoHandler.DeleteProduct(id))
            {
                throw new HttpRequestException("El Producto no ha podido ser borrado correctamente.");
            }
            else return true;
        }
        [HttpGet("{idUsuario}")]
        public List<Producto> TraerProductos(long idUsuario)
        {
            return ProductoHandler.getProductsByUserId(idUsuario);
        }
    }
}
