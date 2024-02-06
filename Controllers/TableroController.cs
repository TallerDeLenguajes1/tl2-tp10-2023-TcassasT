using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_TcassasT.Interfaces;
using tl2_tp10_2023_TcassasT.Models;
using tl2_tp10_2023_TcassasT.ViewModels;

namespace tl2_tp10_2023_TcassasT.Controllers;

[Route("tableros")]
public class TableroController: Controller {
  private readonly ILogger<TableroController> _logger;
  private readonly ITableroReposiroty _tableroReposiroty;
  private readonly ITareaRepository _tareaRepository;
  public TableroController(ILogger<TableroController> logger, ITableroReposiroty tableroRepository, ITareaRepository tareaRepository) {
      _logger = logger;
      _tableroReposiroty = tableroRepository;
      _tareaRepository = tareaRepository;
  }

  [HttpGet("")]
  public IActionResult GetTableros() {
    int userId = 1;
    List<Tablero> tableros = _tableroReposiroty.GetTablerosByUserId(userId);
    return View(tableros);
  }

  [HttpGet("nuevo")]
  public IActionResult CrearTablero() {
    return View(new Tablero());
  }

  [HttpPost("nuevo")]
  public IActionResult CrearTablero(Tablero tablero) {
    _tableroReposiroty.CrearTablero(tablero);
    return RedirectToAction("GetTableros");
  }

  [HttpGet("{id}/modificar")]
  public IActionResult ModificarTablero(int id) {
    Tablero tablero = _tableroReposiroty.GetTablero(id);
    return View(tablero);
  }

  [HttpPost("{id}/modificar")]
  public IActionResult ModificarTablero(int id, Tablero tablero) {
    _tableroReposiroty.ModificarTablero(id, tablero);
    return RedirectToAction("GetTableros");
  }

  [HttpGet("{id}/eliminar")]
  public IActionResult EliminarTablero(int id) {
    _tableroReposiroty.EliminarTablero(id);
    return RedirectToAction("GetTableros");
  }

  // Tareas
  [HttpGet("{idTablero}/tareas")]
  public ActionResult GetTareasByTableroId(int idTablero) {
    GetTareasByTableroIdViewModel vm = new GetTareasByTableroIdViewModel();

    List<Tarea> todasLasTareas = _tareaRepository.GetTareasByTableroId(idTablero);
    foreach (var tarea in todasLasTareas) {
      vm.TareasPorEstado[tarea.Estado].Add(tarea);
    }

    vm.TableroId = idTablero;
    vm.CantidadDeTareas = todasLasTareas.Count();

    return View(vm);
  }

  [HttpGet("{idTablero}/tareas/nueva")]
  public ActionResult CrearTarea(int idTablero) {
    return View(new Tarea() { IdTablero = idTablero });
  }

  [HttpPost("{idTablero}/tareas/nueva")]
  public IActionResult CrearTarea(int idTablero, Tarea tarea) {
    _tareaRepository.CrearTareaEnTablero(tarea);
    return RedirectToAction("GetTareasByTableroId", new { idTablero });
  }

  [HttpGet("{idTablero}/tareas/{idTarea}/modificar")]
  public IActionResult ModificarTarea(int idTablero, int idTarea) {
    Tarea tareaAModificar = _tareaRepository.GetTarea(idTarea);
    return View(tareaAModificar);
  }

  [HttpPost("{idTablero}/tareas/{idTarea}/modificar")]
  public IActionResult ModificarTarea(int idTablero, int idTarea, Tarea tarea) {
    _tareaRepository.ModificarTarea(idTarea, tarea);
    return RedirectToAction("GetTareasByTableroId", new { idTablero });
  }

  [HttpPost("{idTablero}/tareas/{idTarea}/modificar/estado")]
  public IActionResult ModificarEstadoTarea(int idTablero, int idTarea, EstadoTarea estado) {
    _tareaRepository.ModificarEstado(idTarea, estado);
    return RedirectToAction("GetTareasByTableroId", new { idTablero });
  }

  [HttpGet("{idTablero}/tareas/{idTarea}/eliminar")]
  public IActionResult EliminarTarea(int idTablero, int idTarea) {
    Tarea tarea = _tareaRepository.GetTarea(idTarea);
    _tareaRepository.EliminarTarea(idTarea);
    return RedirectToAction("GetTareasByTableroId", new { idTablero });
  }
}
