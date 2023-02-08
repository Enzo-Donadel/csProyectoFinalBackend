namespace SistemaGestionWebApi_EnzoDonadel.Models
{
    public class Usuario
    {
        private long _id;
        private string _nombre;
        private string _apellido;
        private string _nombreUsuario;
        private string _contraseña;
        private string _mail;

        #region Constructores
        public Usuario()
        {
            this._id = 0;
            this._nombre = string.Empty;
            this._apellido = string.Empty;
            this._nombreUsuario = string.Empty;
            this._contraseña = string.Empty;
            this._mail = string.Empty;
        }

        public Usuario(long id, string nombre, string apellido, string nombreUsuario, string contraseña, string mail)
        {
            this._id = id;
            this._nombre = nombre;
            this._apellido = apellido;
            this._nombreUsuario = nombreUsuario;
            this._contraseña = contraseña;
            this._mail = mail;
        }
        #endregion
        #region Getters y Setters
        public long Id { get => _id; set => _id = value; }
        public string Nombre { get => _nombre; set => _nombre = value; }
        public string Apellido { get => _apellido; set => _apellido = value; }
        public string NombreUsuario { get => _nombreUsuario; set => _nombreUsuario = value; }
        public string Contraseña { get => _contraseña; set => _contraseña = value; }
        public string Mail { get => _mail; set => _mail = value; }
        #endregion
    }
}