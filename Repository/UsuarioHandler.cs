using SistemaGestionWebApi_EnzoDonadel.Models;
using System.Data.SqlClient;

namespace SistemaGestionWebApi_EnzoDonadel.Repository
{
    internal static class UsuarioHandler
    {
        //Traer Lista de Todos los usuarios
        public static List<Usuario> GetAllUsuario()
        {
            List<Usuario> userList = new List<Usuario>();
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand("SELECT * FROM Usuario", SqlDbConnection))
                {
                    SqlDbConnection.Open();
                    using (SqlDataReader DataReader = SqlDbQuery.ExecuteReader())
                    {
                        if (DataReader.HasRows)
                        {
                            while (DataReader.Read())
                            {
                                Usuario temp = new Usuario();
                                temp.Id = DataReader.GetInt64(0);
                                temp.Nombre = DataReader.GetString(1);
                                temp.Apellido = DataReader.GetString(2);
                                temp.NombreUsuario = DataReader.GetString(3);
                                temp.Contraseña = DataReader.GetString(4);
                                temp.Mail = DataReader.GetString(5);

                                userList.Add(temp);
                            }
                        }
                    }
                    SqlDbConnection.Close();
                }
            }
            return userList;
        }
        //Traer Usuario (recibe un int)
        public static Usuario GetUsuarioByID(long idToSearch)
        {
            Usuario user = new Usuario();
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                string query = "SELECT * FROM Usuario WHERE Id=@parameterToSearch";
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlParameter ParameterID = new SqlParameter("parameterToSearch", System.Data.SqlDbType.BigInt);
                    ParameterID.Value = idToSearch;
                    SqlDbQuery.Parameters.Add(ParameterID);
                    SqlDbConnection.Open();
                    using (SqlDataReader DataReader = SqlDbQuery.ExecuteReader())
                    {
                        if (DataReader.HasRows)
                        {
                            DataReader.Read();
                            Usuario temp = new Usuario();
                            user.Id = Convert.ToInt64(DataReader.GetInt64(0));
                            user.Nombre = DataReader.GetString(1);
                            user.Apellido = DataReader.GetString(2);
                            user.NombreUsuario = DataReader.GetString(3);
                            user.Contraseña = DataReader.GetString(4);
                            user.Mail = DataReader.GetString(5);
                        }
                    }
                    SqlDbConnection.Close();
                }

            }
            return user;
        }
        //Inicio de sesión (recibe un usuario y contraseña y devuelve un objeto Usuario)
        public static Usuario UserLogIn(string userLogged, string passLogged)
        {
            Usuario user = new Usuario();
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                string query = "SELECT * FROM Usuario WHERE NombreUsuario = @userParameter AND Contraseña = @passParameter";
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    SqlParameter ParameterUser = new SqlParameter("userParameter", System.Data.SqlDbType.VarChar);
                    ParameterUser.Value = userLogged;
                    SqlParameter ParameterPass = new SqlParameter("passParameter", System.Data.SqlDbType.VarChar);
                    ParameterPass.Value = passLogged;
                    SqlDbQuery.Parameters.Add(ParameterUser);
                    SqlDbQuery.Parameters.Add(ParameterPass);
                    SqlDbConnection.Open();
                    using (SqlDataReader DataReader = SqlDbQuery.ExecuteReader())
                    {
                        if (DataReader.HasRows)
                        {
                            DataReader.Read();
                            user.Id = Convert.ToInt64(DataReader.GetInt64(0));
                            user.Nombre = DataReader.GetString(1);
                            user.Apellido = DataReader.GetString(2);
                            user.NombreUsuario = DataReader.GetString(3);
                            user.Contraseña = DataReader.GetString(4);
                            user.Mail = DataReader.GetString(5);
                        }
                    }
                    SqlDbConnection.Close();
                }
            }
            return user;
        }
        //Update de Usuario. Si cualquier valor es "Null" o "vacio", en caso de strings, no lo modifica.
        public static void UpdateUsuario(Usuario DataToUpdate)
        {
            int AffectedRegisters;
            string query = "UPDATE Usuario " +
                "SET " +
                "Nombre = @nameToChange, " +
                "Apellido = @lastNameToChange, " +
                "NombreUsuario = @userNameToChange, " +
                "Contraseña = @passwordToChange, " +
                "Mail = @mailToChange " +
                "WHERE Id = @userId";
            using (SqlConnection SqlDbConnection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand SqlDbQuery = new SqlCommand(query, SqlDbConnection))
                {
                    Usuario original = GetUsuarioByID(DataToUpdate.Id);
                    if (DataToUpdate.Nombre != null)
                    {
                        SqlDbQuery.Parameters.AddWithValue("@nameToChange", DataToUpdate.Nombre);
                    }
                    else SqlDbQuery.Parameters.AddWithValue("nametoChange", original.Nombre);
                    if (DataToUpdate.Apellido != null)
                    {
                        SqlDbQuery.Parameters.AddWithValue("@lastNameToChange", DataToUpdate.Apellido);
                    }
                    else SqlDbQuery.Parameters.AddWithValue("@lastNameToChange", original.Apellido);
                    if (DataToUpdate.NombreUsuario != null)
                    {
                        SqlDbQuery.Parameters.AddWithValue("@userNameToChange", DataToUpdate.NombreUsuario);
                    }
                    else SqlDbQuery.Parameters.AddWithValue("@userNameToChange", original.NombreUsuario);
                    if (DataToUpdate.Contraseña != null)
                    {
                        SqlDbQuery.Parameters.AddWithValue("@passwordToChange", DataToUpdate.Contraseña);
                    }
                    else SqlDbQuery.Parameters.AddWithValue("@passwordToChange", original.Contraseña);
                    if (DataToUpdate.Contraseña != null)
                    {
                        SqlDbQuery.Parameters.AddWithValue("@mailToChange", DataToUpdate.Mail);
                    }
                    else SqlDbQuery.Parameters.AddWithValue("@mailToChange", original.Mail);
                    SqlDbQuery.Parameters.AddWithValue("@userId", DataToUpdate.Id);
                    SqlDbConnection.Open();
                    AffectedRegisters = SqlDbQuery.ExecuteNonQuery();
                    SqlDbConnection.Close();
                }
            }
        }
    }
}
