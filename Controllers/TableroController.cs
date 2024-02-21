using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_TcassasT.Interfaces;
using tl2_tp10_2023_TcassasT.Models;
using tl2_tp10_2023_TcassasT.ViewModels;
using tl2_tp10_2023_TcassasT.Utility;

namespace tl2_tp10_2023_TcassasT.Controllers;

[Route("tableros")]
public class TableroController: Controller {
  private readonly ILogger<TableroController> _logger;
  private readonly ITableroReposiroty _tableroReposiroty;
  private readonly ITareaRepository _tareaRepository;
  private readonly IUsuarioTableroRepository _usuarioTableroRepository;
  private readonly IActividadRepository _actividadRepository;
  private readonly IUsuarioRepository _usuarioRepository;
  public TableroController(ILogger<TableroController> logger, ITableroReposiroty tableroRepository, ITareaRepository tareaRepository, IUsuarioTableroRepository usuarioTableroRepository, IActividadRepository actividadRepository, IUsuarioRepository usuarioRepository) {
      _logger = logger;
      _tableroReposiroty = tableroRepository;
      _tareaRepository = tareaRepository;
      _usuarioTableroRepository = usuarioTableroRepository;
      _actividadRepository = actividadRepository;
      _usuarioRepository = usuarioRepository;
  }

  [HttpGet("")]
  public IActionResult GetTableros(EstatsuGenericoViewModel estatusGenericoVM) {
    int? usuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioLogueado == null) {
      throw new Exception("No existe sesion para identificar usuario creador");
    }

    List<int> tablerosPertenecientes = new List<int>();
    List<Tablero> tableros = _tableroReposiroty.GetTablerosByUserId((int) usuarioLogueado);
    tableros.ForEach((Tablero tablero) => {
      tablerosPertenecientes.Add(tablero.Id);
    });

    List<TableroMembrecias> tablerosConMiembros = _tableroReposiroty.GetTablerosMembreciasByTableroId(tablerosPertenecientes);

    GetTablerosViewModel getTablerosVM = new GetTablerosViewModel() {
      Tableros = tablerosConMiembros,
      Estatus = estatusGenericoVM
    };

    return View(getTablerosVM);
  }

  [HttpGet("nuevo")]
  public IActionResult CrearTablero() {
    CrearTableroViewModel crearTableroVM = new CrearTableroViewModel();
    int? usuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioLogueado == null) {
      crearTableroVM.TieneError = true;
      crearTableroVM.ErrorMensaje = "No existe usuario para identificar como usuario creador";
      return View(crearTableroVM);
    }

    crearTableroVM.IdUsuarioPropietario = (int) usuarioLogueado;

    return View(crearTableroVM);
  }

  [HttpPost("nuevo")]
  public IActionResult CrearTablero(CrearTableroViewModel crearTableroVM) {
    if (!ModelState.IsValid) {
      crearTableroVM.TieneError = true;
      crearTableroVM.ErrorMensaje = "Datos invalidos, por favor reintente";
      return View(crearTableroVM);
    }

    Tablero tablero = new Tablero(crearTableroVM);
    int tableroId = _tableroReposiroty.CrearTablero(tablero);
    _usuarioTableroRepository.AgregarUsuarioATablero(tablero.IdUsuarioPropietario, tableroId);

    EstatsuGenericoViewModel estatusGenericoVM = new EstatsuGenericoViewModel() {
      TieneEstatus = true,
      Severidad = ESTATUS_SEVERIDAD.SUCCESS,
      EstatusMensaje = "Tablero creado con éxito"
    };
    
    return RedirectToAction("GetTableros", estatusGenericoVM);
  }

  [HttpGet("{id}/modificar")]
  public IActionResult ModificarTablero(int id) {
    EstatsuGenericoViewModel estatusGenericoVM = new EstatsuGenericoViewModel();
    Tablero tablero = _tableroReposiroty.GetTablero(id);

    if (tablero == null) {
      estatusGenericoVM.TieneEstatus = true;
      estatusGenericoVM.Severidad = ESTATUS_SEVERIDAD.ERROR;
      estatusGenericoVM.EstatusMensaje = "No se encontró tablero para modificar";
      return RedirectToAction("GetTableros", estatusGenericoVM);
    }

    ModificarTableroViewModel modificarTableroVM = new ModificarTableroViewModel(tablero);

    return View(modificarTableroVM);
  }

  [HttpPost("{id}/modificar")]
  public IActionResult ModificarTablero(int id, ModificarTableroViewModel modificarTableroVM) {
    if (!ModelState.IsValid) {
      modificarTableroVM.TieneError = true;
      modificarTableroVM.ErrorMensaje = "Datos invalidos, por favor reintente";
      return View(modificarTableroVM);
    }

    _tableroReposiroty.ModificarTablero(id, new Tablero(modificarTableroVM));

    EstatsuGenericoViewModel estatusGenericoVM = new EstatsuGenericoViewModel() {
      TieneEstatus = true,
      Severidad = ESTATUS_SEVERIDAD.SUCCESS,
      EstatusMensaje = "Tablero modificado con éxito"
    };

    return RedirectToAction("GetTableros", estatusGenericoVM);
  }

  [HttpGet("{id}/eliminar")]
  public IActionResult EliminarTablero(int id) {
    EstatsuGenericoViewModel estatusGenericoVM = new EstatsuGenericoViewModel();
    Tablero tablero = _tableroReposiroty.GetTablero(id);

    if (tablero == null) {
      estatusGenericoVM.TieneEstatus = true;
      estatusGenericoVM.Severidad = ESTATUS_SEVERIDAD.ERROR;
      estatusGenericoVM.EstatusMensaje = "No se encontró tablero para eliminar";
      return RedirectToAction("GetTableros", estatusGenericoVM);
    }

    _tableroReposiroty.EliminarTablero(id);

    estatusGenericoVM.TieneEstatus = true;
    estatusGenericoVM.Severidad = ESTATUS_SEVERIDAD.SUCCESS;
    estatusGenericoVM.EstatusMensaje = "Tablero eliminado con éxito";

    return RedirectToAction("GetTableros", estatusGenericoVM);
  }

  // Tareas
  [HttpGet("{idTablero}/tareas")]
  public ActionResult GetTareasByTableroId(int idTablero) {
    GetTareasByTableroIdViewModel vm = new GetTareasByTableroIdViewModel();

    Tablero tablero = _tableroReposiroty.GetTablero(idTablero);
    List<Tarea> todasLasTareas = _tareaRepository.GetTareasByTableroId(idTablero);
    foreach (var tarea in todasLasTareas) {
      vm.TareasPorEstado[tarea.Estado].Add(tarea);
    }

    vm.Tablero = tablero;
    vm.CantidadDeTareas = todasLasTareas.Count();

    if (TempData.Get<EstatsuGenericoViewModel>("Estatus") != null) {
      EstatsuGenericoViewModel estatusGenericoVM =  TempData.Get<EstatsuGenericoViewModel>("Estatus");
      vm.Estatus =  estatusGenericoVM;
    }

    return View(vm);
  }

  [HttpGet("{idTablero}/tareas/nueva")]
  public ActionResult CrearTarea(int idTablero) {
    EstatsuGenericoViewModel estatusGenericoVM = new EstatsuGenericoViewModel();
    int? usuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioLogueado == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No existe usuario para identificar como usuario creador");
      return RedirectToAction("GetTareasByTableroId", new { idTablero });
    }

    CrearTareaViewModel crearTareaVM = new CrearTareaViewModel();
    crearTareaVM = new CrearTareaViewModel() {
      IdTablero = idTablero,
      IdUsuarioAsignado = (int) usuarioLogueado
    };

    return View(crearTareaVM);
  }

  [HttpPost("{idTablero}/tareas/nueva")]
  public IActionResult CrearTarea(int idTablero, CrearTareaViewModel crearTareaVM) {
    if (!ModelState.IsValid) {
      crearTareaVM.TieneError = true;
      crearTareaVM.ErrorMensaje = "Datos invalidos, por favor reintente";
      return View(new { idTablero, crearTareaVM });
    }

    _tareaRepository.CrearTareaEnTablero(new Tarea(crearTareaVM));
    
    AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.SUCCESS, "Tarea creada con éxito");

    return RedirectToAction("GetTareasByTableroId", new { idTablero });
  }

  [HttpPost("{idTablero}/tareas/{idTarea}/archivar")]
  public IActionResult ArchivarTarea(int idTablero, int idTarea, ArchivadoTarea archivado) {
    EstatsuGenericoViewModel estatusGenericoVM = new EstatsuGenericoViewModel();
    int? usuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioLogueado == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No existe usuario para identificar actividad");
      return RedirectToAction("GetTareasByTableroId", new { idTablero });
    }

    Tarea tarea = _tareaRepository.GetTarea(idTarea);
    if (tarea == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No existe tarea para archivar");
      return RedirectToAction("GetTareasByTableroId", new { idTablero });
    }

    _actividadRepository.AgregarActividad((int) usuarioLogueado, idTablero, idTarea, (archivado.Equals(ArchivadoTarea.ARCHIVADO) ? "Tarea archivada" : "Tarea des-archivada"));
    _tareaRepository.ModificarArchivado(idTarea, archivado);

    AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.SUCCESS, "Tarea archivada con éxito");

    return RedirectToAction("GetTareasByTableroId", new { idTablero });
  }

  [HttpGet("{idTablero}/tareas/{idTarea}/modificar")]
  public IActionResult ModificarTarea(int idTablero, int idTarea) {
    EstatsuGenericoViewModel estatusGenericoVM = new EstatsuGenericoViewModel();
    Tarea tareaAModificar = _tareaRepository.GetTarea(idTarea);
    if (tareaAModificar == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No existe tarea para archivar");
      return RedirectToAction("GetTareasByTableroId", new { idTablero, estatusGenericoVM });
    }

    ModificarTareaViewModel modificarTareaViewModel = new ModificarTareaViewModel(tareaAModificar);

    return View(modificarTareaViewModel);
  }

  [HttpPost("{idTablero}/tareas/{idTarea}/modificar")]
  public IActionResult ModificarTarea(int idTablero, int idTarea, ModificarTareaViewModel modificarTareaVM) {
    EstatsuGenericoViewModel estatusGenericoVM = new EstatsuGenericoViewModel();
    int? usuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");

    if (!ModelState.IsValid) {
      modificarTareaVM.TieneError = true;
      modificarTareaVM.ErrorMensaje = "Datos invalidos, por favor reintente";
      return View(modificarTareaVM);
    }

    Tarea tarea = _tareaRepository.GetTarea(idTarea);
    if (tarea == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No existe tarea para modificar el estado");
      return RedirectToAction("GetTareasByTableroId", new { idTablero });
    }

    if (usuarioLogueado == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No existe sesion para identificar actividad de usuario");
      return RedirectToAction("GetTareasByTableroId", new { idTablero });
    }

    _tareaRepository.ModificarTarea(idTarea, new Tarea(modificarTareaVM));
    _actividadRepository.AgregarActividad((int) usuarioLogueado, idTablero, idTarea, "Tarea modificada");

    AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.SUCCESS, "Tarea modificada con éxito");

    return RedirectToAction("GetTareasByTableroId", new { idTablero }); 
  }

  [HttpPost("{idTablero}/tareas/{idTarea}/modificar/estado")]
  public IActionResult ModificarEstadoTarea(int idTablero, int idTarea, EstadoTarea estado) {
    EstatsuGenericoViewModel estatusGenericoVM = new EstatsuGenericoViewModel();
    int? usuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");

    Tarea tarea = _tareaRepository.GetTarea(idTarea);
    if (tarea == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No existe tarea para modificar el estado");
      return RedirectToAction("GetTareasByTableroId", new { idTablero, estatusGenericoVM });
    }

    if (usuarioLogueado == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No existe sesion para identificar actividad de usuario");
      return RedirectToAction("GetTareasByTableroId", new { idTablero, estatusGenericoVM }); 
    }

    _actividadRepository.AgregarActividad((int) usuarioLogueado, idTablero, idTarea, "Cambio de estado a " + estado.ToString());
    _tareaRepository.ModificarEstado(idTarea, estado);

    return RedirectToAction("GetTareasByTableroId", new { idTablero });
  }

  [HttpGet("{idTablero}/tareas/{idTarea}/eliminar")]
  public IActionResult EliminarTarea(int idTablero, int idTarea) {
    Tarea tarea = _tareaRepository.GetTarea(idTarea);
    if (tarea == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No existe tarea para eliminar");
      return RedirectToAction("GetTareasByTableroId", new { idTablero });
    }
    _tareaRepository.EliminarTarea(idTarea);
    AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.SUCCESS, "Tarea borrada con éxito");
    return RedirectToAction("GetTareasByTableroId", new { idTablero });
  }

  [HttpGet("{idTablero}/tareas/archivadas")]
  public IActionResult GetTareasArchivadasByTableroId(int idTablero) {
    Tablero tablero = _tableroReposiroty.GetTablero(idTablero);
    List<TareaArchivada> tareas = _tareaRepository.GetTareasArchivadasByTableroId(idTablero);

    GetTareasArchivadasYTableroViewModel tareasYTableroVM = new GetTareasArchivadasYTableroViewModel();
    tareasYTableroVM.Tablero = tablero;
    tareasYTableroVM.Tareas = tareas;

    return View(tareasYTableroVM);
  }

  [HttpGet("{idTablero}/tareas/{idTarea}/actividad")]
  public ICollection<ActividadExtendida> GetActividadByTareaId(int idTablero, int idTarea) {
    List<ActividadExtendida> actividades = _actividadRepository.GetActividadesByTareaId(idTarea);
    return actividades;
  }

  [HttpGet("{idTablero}/actividad")]
  public IActionResult GetActividadByTableroId(int idTablero) {
    Tablero tablero = _tableroReposiroty.GetTablero(idTablero);
    List<ActividadExtendida> actividades = _actividadRepository.GetActividadesByTableroId(idTablero);
    
    GetActividadesByTableroIdViewModel actividadByTableroVM = new GetActividadesByTableroIdViewModel();
    actividadByTableroVM.Tablero = tablero;
    actividadByTableroVM.Actividades = actividades;

    return View(actividadByTableroVM);
  }

  [HttpGet("{idTablero}/miembros")]
  public IActionResult GetMiembrosByTableroId(int idTablero) {
    int? usuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");
    int? usuarioRol = HttpContext.Session.GetInt32("Rol");

    if (usuarioLogueado == null || usuarioRol == null) {
      throw new Exception("No existe sesion para identificar actividad de usuario");
    }

    TableroMembrecias tableroMembrecias = _tableroReposiroty.GetTableroMembreciasByTableroId(idTablero);
    Usuario? usuarioPropietario = tableroMembrecias.Miembros.Find(
      (Usuario usuario) => usuario.Id == tableroMembrecias.IdUsuarioPropietario
    );

    bool puedeAdministrarMiembros = usuarioRol.Equals((int) RolUsuario.ADMINISTRADOR) ||
      (usuarioPropietario != null && usuarioPropietario.Id == usuarioLogueado);

    GetMiembrosByTableroIdViewModel getMiembrosVM = new GetMiembrosByTableroIdViewModel() {
      UsuarioLogueado = (int) usuarioLogueado,
      TableroMembrecias = tableroMembrecias,
      PuedeAdministrarMiembros = puedeAdministrarMiembros,
    };

    if (TempData.Get<EstatsuGenericoViewModel>("Estatus") != null) {
      getMiembrosVM.Estatus = TempData.Get<EstatsuGenericoViewModel>("Estatus");
    }

    return View(getMiembrosVM);
  }

  [HttpGet("{idTablero}/miembros/candidatos")]
  public ICollection<Usuario> GetCandidatosAMiembrosByTableroId(int idTablero, string busqueda) {
    List<Usuario> candidatos = _usuarioRepository.GetCandidatosAMiembrosDeTablero(idTablero, busqueda);
    return candidatos;
  }

  [HttpPost("{idTablero}/miembros/agregar")]
  public IActionResult AgregarMiembroATablero(int idTablero, int idUsuario) {
    Usuario usuario = _usuarioRepository.GetUsuario(idUsuario);
    if (usuario == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No existe usuario para agregar a tablero");
      return RedirectToAction("GetMiembrosByTableroId", new { idTablero });
    }

    if (_usuarioTableroRepository.UsuarioPerteneceATablero(idUsuario, idTablero)) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.WARNING, "Usuario ya pertenece a este tablero");
      return RedirectToAction("GetMiembrosByTableroId", new { idTablero });
    }

    Tablero tablero = _tableroReposiroty.GetTablero(idTablero);
    if (tablero == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No existe tablero");
      return RedirectToAction("GetMiembrosByTableroId", new { idTablero });
    }

    _usuarioTableroRepository.AgregarUsuarioATablero(idUsuario, idTablero);

    AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.SUCCESS, "Usuario agregado con éxito");

    return RedirectToAction("GetMiembrosByTableroId", new { idTablero });
  }

  [HttpPost("{idTablero}/miembros/remover")]
  public IActionResult RemoverMiembroDeTablero(int idTablero, int idUsuario) {
    if (!_usuarioTableroRepository.UsuarioPerteneceATablero(idUsuario, idTablero)) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "Usuario ya pertenece a este tablero");
      return RedirectToAction("GetMiembrosByTableroId", new { idTablero });
    }

    _usuarioTableroRepository.RemoverUsuarioDeTablero(idUsuario, idTablero);

    AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.SUCCESS, "Usuario removido con éxito");

    return RedirectToAction("GetMiembrosByTableroId", new { idTablero });
  }

  [HttpPost("{idTablero}/miembros/propietario")]
  public IActionResult OtorgarPropiedadTablero(int idTablero, int idUsuario) {
    Tablero tablero = _tableroReposiroty.GetTablero(idTablero);

    Usuario nuevoUsuario = _usuarioRepository.GetUsuario(idUsuario);
    if (nuevoUsuario == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No existe usuario para otogar propiedad");
      return RedirectToAction("GetMiembrosByTableroId", new { idTablero });
    }

    if (tablero.IdUsuarioPropietario == nuevoUsuario.Id) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.WARNING, "Usuario ya es propietario");
      return RedirectToAction("GetMiembrosByTableroId", new { idTablero });
    }

    tablero.IdUsuarioPropietario = nuevoUsuario.Id;
    _tableroReposiroty.ModificarTablero(idTablero, tablero);

    AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.SUCCESS, "Propiedad otorgada con éxito");

    return RedirectToAction("GetMiembrosByTableroId", new { idTablero });
  }

  // Metodos utiles
  public void AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD severidad, string mensaje) {
    EstatsuGenericoViewModel estatusGenericoVM = new EstatsuGenericoViewModel();
    estatusGenericoVM.TieneEstatus = true;
    estatusGenericoVM.Severidad = severidad;
    estatusGenericoVM.EstatusMensaje = mensaje;
    TempData.Put("Estatus", estatusGenericoVM);
  }
}
