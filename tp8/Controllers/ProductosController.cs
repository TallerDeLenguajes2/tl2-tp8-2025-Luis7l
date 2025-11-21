using Repository;
using Models;
using Microsoft.AspNetCore.Mvc;
using ViewModels;
public class ProductoController : Controller
{
    ProductoRepository productoRepository;

    public ProductoController()
    {
        productoRepository = new ProductoRepository();
    }
    [HttpGet]
    public IActionResult Index()
    {
        List<Producto> productos = productoRepository.Listar();

        return View(productos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Producto producto)
    {
        if (!ModelState.IsValid)
        {
            return View(producto);
        }  
        var nuevoProducto=new Producto
       {     
            Descripcion=producto.Descripcion,
            Precio=producto.Precio
        };
        productoRepository.Insertar(nuevoProducto);
    

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var producto=productoRepository.Obtener(id);
        if (producto == null)
        {
            return NotFound();
        }
        var productoView=new ProductoViewModel
        {
            Id_producto=producto.IdProducto,
            Descripcion=producto.Descripcion,
            Precio=producto.Precio
        };
        return View(productoView);
    }
    [HttpPost]
    public IActionResult Edit(int id,ProductoViewModel producto)
    {
        if (id != producto.Id_producto)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View (producto);
        }
        var productoAeditar=new Producto
        {
            IdProducto=producto.Id_producto,
            Descripcion=producto.Descripcion,
            Precio=(int)producto.Precio,
        };
        productoRepository.Modificar(id,productoAeditar);
        return  RedirectToAction(nameof(Index));
    }



    [HttpGet]
    public IActionResult delete(int id)
    {
        return View(productoRepository.Obtener(id));
    }
    [HttpPost]
    public IActionResult delete(Producto producto)
    {
        productoRepository.Eliminar(producto.IdProducto);
        return RedirectToAction("Index");
    }

}
    
