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
  private readonly IUsuarioTableroRepository _usuarioTableroRepository;
  private readonly IActividadRepository _actividadRepository;
  public TableroController(ILogger<TableroController> logger, ITableroReposiroty tableroRepository, ITareaRepository tareaRepository, IUsuarioTableroRepository usuarioTableroRepository, IActividadRepository actividadRepository) {
      _logger = logger;
      _tableroReposiroty = tableroRepository;
      _tareaRepository = tareaRepository;
      _usuarioTableroRepository = usuarioTableroRepository;
      _actividadRepository = actividadRepository;
  }

  [HttpGet("")]
  public IActionResult GetTableros() {
    int? usuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioLogueado == null) {
      throw new Exception("No existe sesion para identificar usuario creador");
    }

    List<Tablero> tableros = _tableroReposiroty.GetTablerosByUserId((int) usuarioLogueado);

    return View(tableros);
  }

  [HttpGet("nuevo")]
  public IActionResult CrearTablero() {
    int? usuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioLogueado == null) {
      throw new Exception("No existe sesion para identificar usuario creador");
    }

    Tablero tablero = new Tablero() { IdUsuarioPropietario = (int) usuarioLogueado };

    return View(tablero);
  }

  [HttpPost("nuevo")]
  public IActionResult CrearTablero(Tablero tablero) {
    int? usuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioLogueado == null) {
      throw new Exception("No existe sesion para identificar usuario creador");
    }

    int tableroId = _tableroReposiroty.CrearTablero(tablero);
    _usuarioTableroRepository.AgregarUsuarioATablero((int) usuarioLogueado, tableroId);
    
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

  [HttpPost("{idTablero}/tareas/{idTarea}/archivar")]
  public IActionResult ArchivarTarea(int idTablero, int idTarea, ArchivadoTarea archivado) {
    int? usuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioLogueado == null) {
      throw new Exception("No existe sesion para identificar actividad de usuario");
    }

    _actividadRepository.AgregarActividad((int) usuarioLogueado, idTablero, idTarea, (archivado.Equals(ArchivadoTarea.ARCHIVADO) ? "Tarea archivada" : "Tarea des-archivada"));
    _tareaRepository.ModificarArchivado(idTarea, archivado);
    return RedirectToAction("GetTareasByTableroId", new { idTablero });
  }

  [HttpGet("{idTablero}/tareas/{idTarea}/modificar")]
  public IActionResult ModificarTarea(int idTablero, int idTarea) {
    Tarea tareaAModificar = _tareaRepository.GetTarea(idTarea);
    return View(tareaAModificar);
  }

  [HttpPost("{idTablero}/tareas/{idTarea}/modificar")]
  public IActionResult ModificarTarea(int idTablero, int idTarea, Tarea tarea) {
    int? usuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioLogueado == null) {
      throw new Exception("No existe sesion para identificar actividad de usuario");
    }

    _tareaRepository.ModificarTarea(idTarea, tarea);
    _actividadRepository.AgregarActividad((int) usuarioLogueado, idTablero, idTarea, "Tarea modificada");
    return RedirectToAction("GetTareasByTableroId", new { idTablero });
  }

  [HttpPost("{idTablero}/tareas/{idTarea}/modificar/estado")]
  public IActionResult ModificarEstadoTarea(int idTablero, int idTarea, EstadoTarea estado) {
    int? usuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioLogueado == null) {
      throw new Exception("No existe sesion para identificar actividad de usuario");
    }

    _actividadRepository.AgregarActividad((int) usuarioLogueado, idTablero, idTarea, "Cambio de estado a " + estado.ToString());
    _tareaRepository.ModificarEstado(idTarea, estado);
    return RedirectToAction("GetTareasByTableroId", new { idTablero });
  }

  [HttpGet("{idTablero}/tareas/{idTarea}/eliminar")]
  public IActionResult EliminarTarea(int idTablero, int idTarea) {
    Tarea tarea = _tareaRepository.GetTarea(idTarea);
    _tareaRepository.EliminarTarea(idTarea);
    return RedirectToAction("GetTareasByTableroId", new { idTablero });
  }

  [HttpGet("{idTablero}/actividad")]
  public ICollection<Actividad> GetActividadByTableroId(int idTablero) {
    List<Actividad> actividades = _actividadRepository.GetActividadesByTableroId(idTablero);
    return actividades;
  }

  [HttpGet("{idTablero}/tareas/{idTarea}/actividad")]
  public ICollection<Actividad> GetActividadByTareaId(int idTablero, int idTarea) {
    List<Actividad> actividades = _actividadRepository.GetActividadesByTareaId(idTarea);
    return actividades;
  }
}
