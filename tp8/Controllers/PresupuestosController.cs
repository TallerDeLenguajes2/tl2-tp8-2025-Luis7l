using Models;
using Microsoft.AspNetCore.Mvc;
using Repository;

public class PresupuestoController : Controller
{
    private PresupuestoRepository presupuestoRepository;

    public PresupuestoController()
    {
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
    public IActionResult Create(Presupuesto presupuesto)
    {
        presupuestoRepository.Insertar(presupuesto);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
        return View(presupuestoRepository.Obtener(id));
    }

    [HttpPost]
    public IActionResult Edit(Presupuesto presupuesto)
    {
        
        presupuestoRepository.Modificar(presupuesto.IdPresupuesto,presupuesto);
        return RedirectToAction("Index");
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

}