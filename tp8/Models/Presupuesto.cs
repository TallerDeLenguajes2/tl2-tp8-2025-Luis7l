namespace Models;
public class Presupuesto
{
    public static float Iva = 0.21F;
    private int idPresupuesto;
    private string nombreDestinatario;

    private string fecha_creacion;

    List<PresupuestoDetalle> detalle;

    public Presupuesto()
    {
        nombreDestinatario = string.Empty;
        fecha_creacion = string.Empty;
        idPresupuesto = -1;
        detalle = new List<PresupuestoDetalle>();
    }
    public Presupuesto(string nombre, string fechacreacion)
    {
        idPresupuesto = -1;
        detalle = new List<PresupuestoDetalle>();
        this.fecha_creacion = fechacreacion;
        this.nombreDestinatario = nombre;

    }
    public Presupuesto(int id, string nombre, string fechacreacion)
    {
        this.idPresupuesto = id;
        this.nombreDestinatario = nombre;
        this.fecha_creacion = fechacreacion;
        this.detalle = new List<PresupuestoDetalle>();
    }
    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public string FechaCreacion { get => fecha_creacion; set => fecha_creacion = value; }
    public List<PresupuestoDetalle>Detalle{ get => detalle; set => detalle = value; }
    float MontoPresupuesto()
    {
        return detalle.Select(d => d.Cantidad * d.Producto.Precio).Sum();
    }
    float MontoPresupuestoConIva()
    {
        return MontoPresupuesto() * (1 + Iva);
    }
    int CantiadProductos()
    {
        return detalle.Select(d => d.Cantidad).Sum();
    }
}