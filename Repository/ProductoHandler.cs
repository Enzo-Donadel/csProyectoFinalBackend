using SistemaGestionWebApi_EnzoDonadel.Models;
using System.Data.SqlClient;

namespace SistemaGestionWebApi_EnzoDonadel.Repository
{
    internal class ProductoHandler
    {
        //Traer Productos (recibe un id de usuario y, devuelve una lista con todos los productos cargado por ese usuario)
        public static List<Producto> getProductByUserId(long userIdToSearch)
        {
            List<Producto> products = new List<Producto>();
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand("SELECT * FROM Producto WHERE IdUsuario =@parameterToSearch", SqlDbConnection))
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
                                Producto temp = new Producto();
                                temp.Id = DataReader.GetInt64(0);
                                temp.Descripcion = DataReader.GetString(1);
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
                            product.Descripcion = DataReader.GetString(1);
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
    }
}
