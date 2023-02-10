using SistemaGestionWebApi_EnzoDonadel.Models;
using System.Data.SqlClient;

namespace SistemaGestionWebApi_EnzoDonadel.Repository
{
    internal class VentaHandler
    {
        //Traer Ventas (recibe el id del usuario y devuelve un a lista de Ventas realizadas por ese usuario)
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
        public static Usuario GetUsuarioByVenta(long VentaId)
        {
            long idToSearch = 0;
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                string query = "SELECT Venta.IdUsuario FROM Venta WHERE Id = @parameterToSearch";
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlParameter ParameterID = new SqlParameter("parameterToSearch", System.Data.SqlDbType.BigInt);
                    ParameterID.Value = VentaId;
                    SqlDbQuery.Parameters.Add(ParameterID);
                    SqlDbConnection.Open();
                    using (SqlDataReader DataReader = SqlDbQuery.ExecuteReader())
                    {
                        if (DataReader.HasRows)
                        {
                            DataReader.Read();
                            idToSearch = DataReader.GetInt64(0);
                        }
                    }
                    SqlDbConnection.Close();
                }
            }
            return UsuarioHandler.GetUsuarioByID(idToSearch);
        }
        public static void CrearVenta(long id, List<Producto> productos)
        {
            long idVenta = CrearVenta(id);
            foreach (Producto item in productos)
            {
                int cantidadVendida = item.Stock;
                ProductoVendido temp= new ProductoVendido(item.Id, item.Stock, idVenta);
                int temporal = ProductoVendidoHandler.InsertProductoVendido(temp);
                ProductoHandler.UpdateStockProducto(item.Id, cantidadVendida);
            }
        }
        public static long CrearVenta(long idUsuario)
        {
            long idNuevaVenta;
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
    }
}
