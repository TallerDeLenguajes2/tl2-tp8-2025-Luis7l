namespace Models;
public class Producto
{
    private int idProducto;
    private string descripcion;
    private int precio;

    public Producto()
    {
        idProducto = -1;
        descripcion = string.Empty;
        precio = 0;
    }
    public Producto(string descrpicion, int precio)
    {
        idProducto = 0;
        this.descripcion = descrpicion;
        this.precio = precio;
    }
    public Producto(int id, string descrpicion, int precio)
    {
        this.idProducto = id;
        this.descripcion = descrpicion;
        this.precio = precio;
    }

    public int IdProducto { get => idProducto; set => idProducto = value; }
    public int Precio { get => precio; set => precio = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }

}