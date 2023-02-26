using SistemaGestionWebApi_EnzoDonadel.Models;
using System.Data.SqlClient;

namespace SistemaGestionWebApi_EnzoDonadel.Repository
{
    internal class ProductoVendidoHandler
    {
        public static List<Producto> getProductosInVenta(long idVenta)
        {
            List<Producto> ProductosEnVenta = new List<Producto>();
            List<long> IdProductosEnVenta = new List<long>();
            long temp = 0;
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                string query = "SELECT ProductoVendido.IdProducto FROM ProductoVendido WHERE IdVenta =@parameterToSearch";
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlParameter ParameterID = new SqlParameter("parameterToSearch", System.Data.SqlDbType.BigInt);
                    ParameterID.Value = idVenta;
                    SqlDbQuery.Parameters.Add(ParameterID);
                    SqlDbConnection.Open();
                    using (SqlDataReader DataReader = SqlDbQuery.ExecuteReader())
                    {
                        if (DataReader.HasRows)
                        {
                            while (DataReader.Read())
                            {

                                temp = DataReader.GetInt64(0);
                                IdProductosEnVenta.Add(temp);
                            }
                        }
                    }
                    SqlDbConnection.Close();
                }
            }
            foreach (long item in IdProductosEnVenta)
            {
                ProductosEnVenta.Add(ProductoHandler.getProductById(item));
            }
            return ProductosEnVenta;
        }
        public static int getCantidadDeProductosVendidos(long idVenta, long idProducto)
        {
            int result = 0;
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                string query = "SELECT ProductoVendido.Stock FROM ProductoVendido WHERE IdVenta = @parameter1ToSearch AND IdProducto = @parameter2ToSearch";
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlParameter Parameter1ID = new SqlParameter("parameter1ToSearch", System.Data.SqlDbType.BigInt);
                    Parameter1ID.Value = idVenta;
                    SqlDbQuery.Parameters.Add(Parameter1ID);
                    SqlParameter Parameter2ID = new SqlParameter("parameter2ToSearch", System.Data.SqlDbType.BigInt);
                    Parameter2ID.Value = idProducto;
                    SqlDbQuery.Parameters.Add(Parameter2ID);
                    SqlDbConnection.Open();
                    using (SqlDataReader DataReader = SqlDbQuery.ExecuteReader())
                    {
                        if (DataReader.HasRows)
                        {
                            DataReader.Read();
                            result += DataReader.GetInt32(0);
                        }
                    }
                    SqlDbConnection.Close();
                }
            }
            return result;
        }
        #region Metodos Proyecto Final
        public static List<ProductoVendido> GetProductoVendidoByUserName(long idUsuario)
        {
            List<ProductoVendido> productoVendidos = new List<ProductoVendido>();
            List<Venta> VentasDeUsuario = VentaHandler.GetVentaByUserId(idUsuario);
            foreach (Venta venta in VentasDeUsuario)
            {
                List<ProductoVendido> temp = ProductoVendidoHandler.getProductoVendidoByVenta(venta.Id);
                productoVendidos.AddRange(temp);
            }
            return productoVendidos;
        }
        #endregion
        #region Metodos Helper internos
        internal static List<ProductoVendido> getProductoVendidoByVenta(long sellIdToSearch)
        {
            List<ProductoVendido> productosDeVentaX = new List<ProductoVendido>();
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                string query = "SELECT * FROM ProductoVendido WHERE IdVenta =@parameterToSearch";
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlParameter ParameterID = new SqlParameter("parameterToSearch", System.Data.SqlDbType.BigInt);
                    ParameterID.Value = sellIdToSearch;
                    SqlDbQuery.Parameters.Add(ParameterID);
                    SqlDbConnection.Open();
                    using (SqlDataReader DataReader = SqlDbQuery.ExecuteReader())
                    {
                        if (DataReader.HasRows)
                        {
                            while (DataReader.Read())
                            {
                                ProductoVendido temp = new ProductoVendido();
                                temp.Id = DataReader.GetInt64(0);
                                temp.Stock = Convert.ToInt32(DataReader.GetInt32(1));
                                temp.IdProducto = DataReader.GetInt64(2);
                                temp.IdVenta = DataReader.GetInt64(3);
                                productosDeVentaX.Add(temp);
                            }
                        }
                    }
                    SqlDbConnection.Close();
                }
            }
            return productosDeVentaX;
        }
        internal static bool DeleteProductoVendidoByProductID(long idToDelete)
        {
            bool result = false;
            //Metodo Helper necesario para el funcionamiento de "BorrarProductoByUser", ya que previamente se deben borrar los productosVendidos de dicho producto.
            string query = "Delete FROM ProductoVendido " +
                            "WHERE " +
                                "IdProducto = @idParameter";
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlDbQuery.Parameters.AddWithValue("@idParameter", idToDelete);
                    SqlDbConnection.Open();
                    int affectedRows =SqlDbQuery.ExecuteNonQuery() ;
                    SqlDbConnection.Close();
                    result = true;
                }
            }
            return result;
        }
        internal static bool InsertProductoVendido(ProductoVendido productToAdd)
        {
            bool result = false;
            string query = "INSERT INTO ProductoVendido " +
                                "(Stock,IdProducto, IdVenta) " +
                            "VALUES " +
                                "(@stockToADD,@idProductoToADD, @idVentaToADD); " +
                            "SELECT @@IDENTITY";
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlDbQuery.Parameters.AddWithValue("@stockToADD", productToAdd.Stock);
                    SqlDbQuery.Parameters.AddWithValue("@idProductoToADD", productToAdd.IdProducto);
                    SqlDbQuery.Parameters.AddWithValue("@idVentaToADD", productToAdd.IdVenta);
                    SqlDbConnection.Open();
                    if (SqlDbQuery.ExecuteNonQuery() == 1)
                    {
                        result = true;
                    }
                    SqlDbConnection.Close();
                }
            }
            return result;
        }
        //Metodo Helper necesario para el funcionamiento de "BorrarVentaByUser", ya que previamente se deben borrar los productosVendidos en dicha venta.
        internal static void DeleteProductoVendidoByVentaId(long idToDelete)
        {
            int AffectedRegisters;
            string query = "Delete FROM ProductoVendido " +
                            "WHERE " +
                                "IdVenta = @idParameter";
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlDbQuery.Parameters.AddWithValue("@idParameter", idToDelete);
                    SqlDbConnection.Open();
                    AffectedRegisters = SqlDbQuery.ExecuteNonQuery();
                    SqlDbConnection.Close();
                }
            }
        }
        #endregion
    }
}
