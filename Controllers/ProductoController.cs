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
        public void CrearProducto(Producto DataToAdd)
        {
            ProductoHandler.AddProducto(DataToAdd);
        }
        [HttpPut]
        public void ModificarProducto(Producto DataToUpdate)
        {
            ProductoHandler.UpdateProducto(DataToUpdate);
        }
        [HttpDelete("{id}")]
        public void BorrarProducto(long id)
        {
            ProductoHandler.DeleteProduct(id);
        }
    }
}
