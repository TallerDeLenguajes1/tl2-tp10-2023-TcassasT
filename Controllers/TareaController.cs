using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_TcassasT.Interfaces;
using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.Controllers;

public class TareaController: Controller {
  private readonly ILogger<TareaController> _logger;
  private readonly ITareaRepository tareaRepository;
  public TareaController(ILogger<TareaController> logger) {
      _logger = logger;
      tareaRepository = new TareaRepository();
  }

  [HttpGet]
  public ActionResult GetTareasByTableroId(int id) {
    List<Tarea> tareas = tareaRepository.GetTareasByTableroId(id);
    return View(tareas);
  }

  [HttpGet]
  public ActionResult CrearTarea() {
    return View(new Tarea());
  }

  [HttpPost]
  public IActionResult CrearTarea(Tarea tarea) {
    tareaRepository.CrearTareaEnTablero(tarea);
    return RedirectToAction("GetTareasByTableroId", new { id = tarea.IdTablero });
  }

  [HttpGet]
  public IActionResult ModificarTarea(int id) {
    Tarea tareaAModificar = tareaRepository.GetTarea(id);
    return View(tareaAModificar);
  }

  [HttpPost]
  public IActionResult ModificarTarea(int id, Tarea tarea) {
    tareaRepository.ModificarTarea(id, tarea);
    return RedirectToAction("GetTareasByTableroId", new { id = id });
  }

  public IActionResult EliminarTarea(int id) {
    Tarea tarea = tareaRepository.GetTarea(id);
    tareaRepository.EliminarTarea(id);
    return RedirectToAction("GetTareasByTableroId", new { id = tarea.IdTablero });
  }
}