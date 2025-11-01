using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Luis7l.Models;

namespace tl2_tp8_2025_Luis7l.Controllers;
public class ProductosController : Controller
{
    private ProductoRepository productoRepository;
    public ProductosController()
    {
        productoRepository = new ProductoRepository();
    }
    //A partir de aquí van todos los Action Methods (Get, Post,etc.)
}
