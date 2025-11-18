namespace Models;

public class PresupuestoDetalle
{
    private Producto producto;
    private int cantidad;

    public PresupuestoDetalle()
    {
        cantidad = 0;
        producto = new Producto();
    }

    public PresupuestoDetalle(int cant, Producto p)
    {
        this.cantidad = cant;
        this.producto = p;
    }
    
    public int Cantidad { get => cantidad; set=> cantidad=value; }
    public Producto Producto{ get => producto; set => producto = value; }
}