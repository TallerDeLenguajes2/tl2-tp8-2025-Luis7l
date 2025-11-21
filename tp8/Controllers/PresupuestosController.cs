using Models;
using Microsoft.AspNetCore.Mvc;
using Repository;
using ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

public class PresupuestoController : Controller
{
    private PresupuestoRepository presupuestoRepository;
    private ProductoRepository productoRepository;

    public PresupuestoController()
    {
        productoRepository=new ProductoRepository();
        presupuestoRepository = new PresupuestoRepository();
    }
    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuesto> presupuestos = presupuestoRepository.Listar();

        return View(presupuestos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View ();
    }
    [HttpPost]
    public IActionResult Create(PresupuestoViewModel presupuesto)
    {
        if (presupuesto.FechaCreacion > DateTime.Today)
        {
            ModelState.AddModelError("FechaCreacion", "La fecha no puede ser futura");
        }
        if (!ModelState.IsValid)
        {
            return View(presupuesto);
        }
        var nuevoPresupuesto=new Presupuesto
        {
            NombreDestinatario=presupuesto.NombreDestinatario,
            FechaCreacion=presupuesto.FechaCreacion,
            Detalle=new List<PresupuestoDetalle>( )
        };

        presupuestoRepository.Insertar(nuevoPresupuesto);
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var presupuesto = presupuestoRepository.Obtener(id);

        if (presupuesto == null) return NotFound();

        var presupuestoVM = new PresupuestoViewModel
        {
            Id_presupuesto=presupuesto.IdPresupuesto,
            NombreDestinatario=presupuesto.NombreDestinatario,
            FechaCreacion=presupuesto.FechaCreacion
        };

        return View(presupuestoVM);
        
    }

    [HttpPost]
    public IActionResult Edit(int id, PresupuestoViewModel presupuesto)
    {

        if(id!=presupuesto.Id_presupuesto)return NotFound();
        if (presupuesto.FechaCreacion > DateTime.Today)
        {
            ModelState.AddModelError("Fecha de creacion", "La fecha no puede ser futura");
        }
        if(!ModelState.IsValid){return View(presupuesto);}
        var presupuestoEditado= new Presupuesto
        {
            IdPresupuesto=presupuesto.Id_presupuesto,
            NombreDestinatario=presupuesto.NombreDestinatario,
            FechaCreacion=presupuesto.FechaCreacion
        };
        presupuestoRepository.Modificar(presupuestoEditado.IdPresupuesto,presupuestoEditado);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
       return View(presupuestoRepository.Obtener(id));   
    }
    
    [HttpPost]

    public IActionResult Delete(Presupuesto presupuesto)
    {   
        presupuestoRepository.Eliminar(presupuesto.IdPresupuesto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public IActionResult Details(int id)
    {
        Presupuesto presupuesto = presupuestoRepository.Obtener(id);
        if (presupuesto == null)
        {
            return NotFound();
        }
        return View(presupuesto);
    }

    [HttpGet]
    public IActionResult AgregarProducto(int id)
    {
        List<Producto> productos=productoRepository.Listar();
        var  model =new AgregarProductoViewModel
        {
            Id_presupuesto=id,
            ListaProductos=new SelectList(productos,"IdProducto","Descripcion")
        };
        return View(model);
    }
    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var producto=productoRepository.Listar();
            model.ListaProductos=new SelectList(producto,"IdProducto","Descripcion");
            return View(model);
        }

        presupuestoRepository.InsertarDetalle(model.Id_presupuesto,model.Id_producto,model.Cantidad);
        return RedirectToAction(nameof(Details),new {id=model.Id_presupuesto});
    }
    


}