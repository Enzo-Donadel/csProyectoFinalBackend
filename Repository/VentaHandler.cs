using SistemaGestionWebApi_EnzoDonadel.Models;
using System.Data.SqlClient;

namespace SistemaGestionWebApi_EnzoDonadel.Repository
{
    internal class VentaHandler
    {
        #region Metodos ProyectoFinal
        public static List<Venta> GetVentaByUserId(long userIdToSearch)
        {
            List<Venta> ventas = new List<Venta>();
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand("SELECT * FROM Venta WHERE IdUsuario =@parameterToSearch", SqlDbConnection))
                {
                    SqlParameter ParameterID = new SqlParameter("parameterToSearch", System.Data.SqlDbType.BigInt);
                    ParameterID.Value = userIdToSearch;
                    SqlDbQuery.Parameters.Add(ParameterID);
                    SqlDbConnection.Open();
                    using (SqlDataReader DataReader = SqlDbQuery.ExecuteReader())
                    {
                        if (DataReader.HasRows)
                        {
                            while (DataReader.Read())
                            {
                                Venta temp = new Venta();
                                temp.Id = DataReader.GetInt64(0);
                                temp.Comentarios = DataReader.GetString(1);
                                temp.IdUsuario = DataReader.GetInt64(2);
                                ventas.Add(temp);
                            }
                        }
                    }
                    SqlDbConnection.Close();
                }
            }
            return ventas;
        }
        public static bool CrearVenta(long id, List<Producto> productos)
        {
            long idVenta = CrearVenta(id);
            if (idVenta == 0)
            {
                return false;
            }
            foreach (Producto item in productos)
            {
                int cantidadVendida = item.Stock;
                ProductoVendido temp = new ProductoVendido(item.Id, item.Stock, idVenta);
                if (!ProductoVendidoHandler.InsertProductoVendido(temp))
                {
                    return false;
                }
                if (!ProductoHandler.UpdateStockProducto(item.Id, cantidadVendida))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
        #region Metodos Helper internos
        internal static long CrearVenta(long idUsuario)
        {
            long idNuevaVenta = 0;
            string query = "INSERT INTO Venta " +
                                "(Comentarios, IdUsuario) " +
                            "VALUES " +
                                "(@comentarioToADD, @IdUsuarioToADD); " +
                            "SELECT @@IDENTITY";
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlDbQuery.Parameters.AddWithValue("@comentarioToADD", "");
                    SqlDbQuery.Parameters.AddWithValue("@IdUsuarioToADD", idUsuario);
                    SqlDbConnection.Open();
                    idNuevaVenta = Convert.ToInt64(SqlDbQuery.ExecuteScalar());
                    SqlDbConnection.Close();
                }
            }
            return idNuevaVenta;
        }
        //Metodo Helper necesario para el funcionamiento de "BorrarUsuario", ya que previamente se deben borrar las ventas cargados por ese Usuario.
        internal static bool DeleteVentasByUser(long idUsuario)
        {
            bool result = false;
            int AffectedRegisters;
            //Previamente se deben eliminar todos los productos vendidos del producto en cuestion.
            List<Venta> Ventas = VentaHandler.GetVentaByUserId(idUsuario);
            foreach (Venta venta in Ventas)
            {
                ProductoVendidoHandler.DeleteProductoVendidoByVentaId(venta.Id);
            }
            string query = "Delete FROM Venta " +
                            "WHERE " +
                                "IdUsuario = @idParameter";
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlDbQuery.Parameters.AddWithValue("@idParameter", idUsuario);
                    SqlDbConnection.Open();
                    AffectedRegisters = SqlDbQuery.ExecuteNonQuery();
                    if (AffectedRegisters > 0)
                    {
                        result = true;
                    }
                    SqlDbConnection.Close();
                }
            }
            return result;
        }
        #endregion
    }
}
