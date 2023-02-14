using SistemaGestionWebApi_EnzoDonadel.Models;
using System.Data.SqlClient;

namespace SistemaGestionWebApi_EnzoDonadel.Repository
{
    internal class ProductoHandler
    {
        public static Producto getProductById(long IdToSearch)
        {
            Producto product = new Producto();
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand("SELECT * FROM Producto WHERE Id =@parameterToSearch", SqlDbConnection))
                {
                    SqlParameter ParameterID = new SqlParameter("parameterToSearch", System.Data.SqlDbType.BigInt);
                    ParameterID.Value = IdToSearch;
                    SqlDbQuery.Parameters.Add(ParameterID);
                    SqlDbConnection.Open();
                    using (SqlDataReader DataReader = SqlDbQuery.ExecuteReader())
                    {
                        if (DataReader.HasRows)
                        {
                            DataReader.Read();
                            product.Id = DataReader.GetInt64(0);
                            product.Descripciones = DataReader.GetString(1);
                            product.Costo = DataReader.GetDecimal(2);
                            product.PrecioVenta = DataReader.GetDecimal(3);
                            product.Stock = DataReader.GetInt32(4);
                            product.IdUsuario = DataReader.GetInt64(5);
                        }
                    }
                    SqlDbConnection.Close();
                }
            }
            return product;
        }
        public static Dictionary<Producto, int> getProductosVendidoPorUsuario(long userId)
        {
            Dictionary<Producto, int> products = new Dictionary<Producto, int>();
            List<Venta> ventasDeUsuario = VentaHandler.GetVentaByUserId(userId);
            foreach (Venta venta in ventasDeUsuario)
            {
                List<Producto> temp = ProductoVendidoHandler.getProductosInVenta(venta.Id);
                foreach (Producto product in temp)
                {
                    if (!products.ContainsKey(product))
                    {
                        products.Add(product, ProductoVendidoHandler.getCantidadDeProductosVendidos(venta.Id, product.Id));
                    }
                    else
                    {
                        int temporalcounter = 0;
                        products.TryGetValue(product, out temporalcounter);
                        products[product] = temporalcounter + ProductoVendidoHandler.getCantidadDeProductosVendidos(venta.Id, product.Id);
                    }
                }
            }
            return products;
        }
        #region Metodos Proyecto Final
        //Traer Productos (recibe un id de usuario y, devuelve una lista con todos los productos cargado por ese usuario)
        public static List<Producto> getProductsByUserId(long userIdToSearch)
        {
            List<Producto> products = new List<Producto>();
            string query = "SELECT * FROM Producto WHERE IdUsuario =@parameterToSearch";
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlDbQuery.Parameters.AddWithValue("@parameterToSearch", userIdToSearch);
                    SqlDbConnection.Open();
                    using (SqlDataReader DataReader = SqlDbQuery.ExecuteReader())
                    {
                        if (DataReader.HasRows)
                        {
                            while (DataReader.Read())
                            {
                                Producto temp = new Producto();
                                temp.Id = DataReader.GetInt64(0);
                                temp.Descripciones = DataReader.GetString(1);
                                temp.Costo = DataReader.GetDecimal(2);
                                temp.PrecioVenta = DataReader.GetDecimal(3);
                                temp.Stock = DataReader.GetInt32(4);
                                temp.IdUsuario = DataReader.GetInt64(5);
                                products.Add(temp);
                            }
                        }
                    }
                    SqlDbConnection.Close();
                }
            }
            return products;
        }
        public static bool AddProducto(Producto productToAdd)
        {
            bool result = false;
            string query = "INSERT INTO Producto " +
                                "(Descripciones, Costo, PrecioVenta, Stock, IdUsuario) " +
                            "VALUES " +
                                "(@DescriptionToADD, @CostoToADD, @PrecioVentaToADD, @StockToADD, @IdUsuarioToADD)";
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlDbQuery.Parameters.AddWithValue("@DescriptionToADD", productToAdd.Descripciones);
                    SqlDbQuery.Parameters.AddWithValue("@CostoToADD", productToAdd.Costo);
                    SqlDbQuery.Parameters.AddWithValue("@PrecioVentaToADD", productToAdd.PrecioVenta);
                    SqlDbQuery.Parameters.AddWithValue("@StockToADD", productToAdd.Stock);
                    SqlDbQuery.Parameters.AddWithValue("@IdUsuarioToADD", productToAdd.IdUsuario);
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
        public static bool DeleteProduct(long idToDelete)
        {
            bool result = false;
            //Previamente se deben eliminar todos los productos vendidos del producto en cuestion.
            if (!ProductoVendidoHandler.DeleteProductoVendidoByProductID(idToDelete))
            {
                return false;
            }
            string query = "Delete FROM Producto " +
                            "WHERE " +
                                "Id = @idParameter";
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlDbQuery.Parameters.AddWithValue("@idParameter", idToDelete);
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
        public static bool UpdateProducto(Producto DataToUpdate)
        {
            bool result = false;
            string query = "UPDATE Producto " +
                            "SET " +
                                "Descripciones = @descriptionToChange, " +
                                "Costo = @costoToChange, " +
                                "PrecioVenta = @precioVentaToChange, " +
                                "Stock = @stockToChange, " +
                                "IdUsuario = @idUsuarioToChange " +
                                "WHERE Id = @ProductId";
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    Producto original = getProductById(DataToUpdate.Id);
                    SqlDbQuery.Parameters.AddWithValue("@descriptionToChange", DataToUpdate.Descripciones);
                    SqlDbQuery.Parameters.AddWithValue("@costoToChange", DataToUpdate.Costo);
                    SqlDbQuery.Parameters.AddWithValue("@precioVentaToChange", DataToUpdate.PrecioVenta);
                    SqlDbQuery.Parameters.AddWithValue("@stockToChange", DataToUpdate.Stock);
                    SqlDbQuery.Parameters.AddWithValue("@idUsuarioToChange", DataToUpdate.IdUsuario);
                    SqlDbQuery.Parameters.AddWithValue("@ProductId", DataToUpdate.Id);
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
        #endregion
        #region Metodos Internos
        //Necesarios para otros metodos
        internal static bool UpdateStockProducto(long idProducto, int StockVendido)
        {
            bool result = false;
            Producto productSelled = ProductoHandler.getProductById(idProducto);
            productSelled.Stock -= StockVendido;
            string query = "UPDATE Producto " +
                            "SET " +
                                "Stock = @stockToChange " +
                                "WHERE Id = @ProductId";
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlDbQuery.Parameters.AddWithValue("@stockToChange", productSelled.Stock);
                    SqlDbQuery.Parameters.AddWithValue("@ProductId", productSelled.Id);
                    SqlDbConnection.Open();
                    if(SqlDbQuery.ExecuteNonQuery() ==1)
                    {
                        result = true;
                    }
                    SqlDbConnection.Close();
                }
            }
            return result;
        }
        //Metodo Helper necesario para el funcionamiento de "BorrarUsuario", ya que previamente se deben borrar los productos cargados por ese Usuario.
        internal static bool DeleteProductsByUser(long idUsuario)
        {
            bool result = false;
            int AffectedRegisters;
            //Previamente se deben eliminar todos los productos vendidos del producto en cuestion.
            List<Producto> Productos = ProductoHandler.getProductsByUserId(idUsuario);
            foreach (Producto producto in Productos)
            {
                ProductoVendidoHandler.DeleteProductoVendidoByProductID(producto.Id);
            }
            string query = "Delete FROM Producto " +
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
