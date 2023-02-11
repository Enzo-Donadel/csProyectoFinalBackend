namespace SistemaGestionWebApi_EnzoDonadel.Models
{
    public class Producto
    {
        private long _id;
        private string _descripcion;
        private decimal _costo;
        private decimal _precioVenta;
        private int _stock;
        private long _idUsuario;

        public Producto()
        {
            this._id = 0;
            this._descripcion = string.Empty;
            this._costo = 0;
            this._precioVenta = 0;
            this._stock = 0;
            this._idUsuario = 0;
        }

        public Producto(long id, string descripcion, decimal costo, decimal precioVenta, int stock, long idUsuario)
        {
            this._id = id;
            this._descripcion = descripcion;
            this._costo = costo;
            this._precioVenta = precioVenta;
            this._stock = stock;
            this._idUsuario = idUsuario;
        }

        public long Id { get => _id; set => _id = value; }
        public string Descripciones { get => _descripcion; set => _descripcion = value; }
        public decimal Costo { get => _costo; set => _costo = value; }
        public decimal PrecioVenta { get => _precioVenta; set => _precioVenta = value; }
        public int Stock { get => _stock; set => _stock = value; }
        public long IdUsuario { get => _idUsuario; set => _idUsuario = value; }
    }
}