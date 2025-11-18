using Repository;
using Models;
using Microsoft.AspNetCore.Mvc;

public class ProductoController : Controller
{
    private ProductoRepository productoRepository;
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
        productoRepository.Insertar(producto);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        return View(productoRepository.Obtener(id));
    }
    [HttpPost]
    public IActionResult Edit(Producto producto)
    {
        productoRepository.Modificar(producto.IdProducto, producto);
        return RedirectToAction("Index");
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
    
