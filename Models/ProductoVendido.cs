namespace SistemaGestionWebApi_EnzoDonadel.Models
{
    public class ProductoVendido
    {
        private long _id;
        private long _idProducto;
        private int _stock;
        private long _idVenta;

        public ProductoVendido()
        {
            this._id = 0;
            this._idProducto = 0;
            this._stock = 0;
            this._idVenta = 0;
        }

        public ProductoVendido(long id, long idProducto, int stock, long idVenta)
        {
            this._id = id;
            this._idProducto = idProducto;
            this._stock = stock;
            this._idVenta = idVenta;
        }

        public long Id { get => _id; set => _id = value; }
        public long IdProducto { get => _idProducto; set => _idProducto = value; }
        public int Stock { get => _stock; set => _stock = value; }
        public long IdVenta { get => _idVenta; set => _idVenta = value; }
    }
}