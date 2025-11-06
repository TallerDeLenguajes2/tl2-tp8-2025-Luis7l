using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models;
using Persistence;
namespace tl2_tp8_2025_Luis7l.Controllers;

public class ProductoController : Controller
{
    private readonly ILogger<ProductoController> _logger;

    // 1. Cambia el tipo de IRepository<> a la clase concreta ProductoRepository
    private readonly ProductoRepository repositorioProductos;

    public ProductoController(ILogger<ProductoController> logger)
    {
        _logger = logger;


        repositorioProductos = new ProductoRepository();
    }

    [HttpGet]
    public IActionResult Listar()
    {
        // 3. Llama al método correcto: ListarProductos()
        return View(repositorioProductos.ListarProductos());
    }
}