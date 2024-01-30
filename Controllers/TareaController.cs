using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_TcassasT.Interfaces;
using tl2_tp10_2023_TcassasT.Models;
using tl2_tp10_2023_TcassasT.ViewModels;

namespace tl2_tp10_2023_TcassasT.Controllers;

public class TareaController: Controller {
  private readonly ILogger<TareaController> _logger;
  private readonly ITareaRepository _tareaRepository;
  public TareaController(ILogger<TareaController> logger, ITareaRepository tareaRepository) {
      _logger = logger;
      _tareaRepository = tareaRepository;
  }

  [HttpGet]
  public IActionResult ModificarTarea(int id) {
    Tarea tareaAModificar = _tareaRepository.GetTarea(id);
    return View(tareaAModificar);
  }

  [HttpPost]
  public IActionResult ModificarTarea(int id, Tarea tarea) {
    _tareaRepository.ModificarTarea(id, tarea);
    return RedirectToAction("GetTareasByTableroId", new { id = id });
  }

  public IActionResult EliminarTarea(int id) {
    Tarea tarea = _tareaRepository.GetTarea(id);
    _tareaRepository.EliminarTarea(id);
    return RedirectToAction("GetTareasByTableroId", new { id = tarea.IdTablero });
  }
}