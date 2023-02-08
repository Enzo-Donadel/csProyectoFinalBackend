namespace SistemaGestionWebApi_EnzoDonadel.Models
{
    public class Venta
    {
        private long _id;
        private string _comentarios;
        private long _idUsuario;

        public Venta()
        {
            this._id = 0;
            this._comentarios = string.Empty;
            this._idUsuario = 0;
        }

        public Venta(long id, string comentarios, long idUsuario)
        {
            this._id = id;
            this._comentarios = comentarios;
            this._idUsuario = idUsuario;
        }

        public long Id { get => _id; set => _id = value; }
        public string Comentarios { get => _comentarios; set => _comentarios = value; }
        public long IdUsuario { get => _idUsuario; set => _idUsuario = value; }
    }
}