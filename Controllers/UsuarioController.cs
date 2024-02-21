using tl2_tp10_2023_TcassasT.Models;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_TcassasT.Interfaces;
using tl2_tp10_2023_TcassasT.ViewModels;
using tl2_tp10_2023_TcassasT.Utility;

namespace tl2_tp10_2023_TcassasT.Controllers;

[Route("usuarios")]
public class UsuarioController : Controller {
  private readonly ILogger<UsuarioController> _logger;
  private readonly IUsuarioRepository _usuarioRepository;
  public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuarioRepository) {
      _logger = logger;
      _usuarioRepository = usuarioRepository;
  }

  [HttpGet]
  public IActionResult GetUsuarios() {
    int? rol = HttpContext.Session.GetInt32("Rol");
    if (rol != (int) RolUsuario.ADMINISTRADOR) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No tienes permiso para realizar esta acción");
      return RedirectToAction("Index", "Home");
    }

    GetUsuariosViewModel getUsuariosVM = new GetUsuariosViewModel();
    getUsuariosVM.Usuarios = _usuarioRepository.GetUsuarios();
  
    if (TempData.Get<EstatsuGenericoViewModel>("Estatus") != null) {
      getUsuariosVM.Estatus = TempData.Get<EstatsuGenericoViewModel>("Estatus");
    }

    return View(getUsuariosVM);
  }

  [HttpGet("nuevo")]
  public IActionResult CrearUsuario() {
    int? rol = HttpContext.Session.GetInt32("Rol");

    CrearUsuarioViewModel crearUsuarioVM = new CrearUsuarioViewModel() {
      EsAdministrador = rol.Equals((int) RolUsuario.ADMINISTRADOR),
    };

    return View(crearUsuarioVM);
  }

  [HttpPost("nuevo")]
  public IActionResult CrearUsuario(CrearUsuarioViewModel crearUsuarioVM) {
    try {
      _usuarioRepository.CrearUsuario(new Usuario(crearUsuarioVM));
    } catch(Exception e) {
      crearUsuarioVM.TieneError = true;
      crearUsuarioVM.ErrorMensaje = e.Message;
      return View(crearUsuarioVM);
    }

    return RedirectToAction("Login");
  }

  [HttpGet("modificar/{id}")]
  public IActionResult ModificarUsuario(int id) {
    int? rol = HttpContext.Session.GetInt32("Rol");
    if (rol != (int) RolUsuario.ADMINISTRADOR) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No tienes permiso para realizar esta acción");
      return RedirectToAction("Index", "Home");
    }

    Usuario usuarioAModificar = _usuarioRepository.GetUsuario(id);
    if (usuarioAModificar == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No se encontró usuario a modificar");
      return RedirectToAction("GetUsuarios");
    }

    Usuario usuario = _usuarioRepository.GetUsuario(id);

    ModificarUsuarioViewModel modificarUsuarioVM = new ModificarUsuarioViewModel(usuario);

    return View(modificarUsuarioVM);
  }

  [HttpPost("modificar/{id}")]
  public IActionResult ModificarUsuario(int id, ModificarUsuarioViewModel modificarUsuarioVM) {
    int? usuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");
    int? rol = HttpContext.Session.GetInt32("Rol");
    if (rol != (int) RolUsuario.ADMINISTRADOR) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No tienes permiso para realizar esta acción");
      return RedirectToAction("Index", "Home");
    }

    if (!ModelState.IsValid) {
      modificarUsuarioVM.TieneError = true;
      modificarUsuarioVM.ErrorMensaje = "Datos invalidos, por favor reintente";
      return View(modificarUsuarioVM);
    }

    Usuario usuarioAModificar = _usuarioRepository.GetUsuario(modificarUsuarioVM.Id);
    if (usuarioAModificar == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No se encontró usuario a modificar");
      return RedirectToAction("GetUsuarios");
    }

    Usuario usuarioPorNombre = _usuarioRepository.GetUsuario(modificarUsuarioVM.NombreDeUsuario);
    if (usuarioPorNombre != null && usuarioPorNombre.Id != modificarUsuarioVM.Id) {
      ModificarUsuarioViewModel restoreModificarUsuarioVM = new ModificarUsuarioViewModel(usuarioAModificar) {
        TieneError = true,
        ErrorMensaje = "El nombre de usuario '" + modificarUsuarioVM.NombreDeUsuario + "' ya está en uso"
      };
      return View(restoreModificarUsuarioVM);
    }

    _usuarioRepository.ModificarUsuario(id, new Usuario(modificarUsuarioVM));
    AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.SUCCESS, "Usuario modificado con éxito");

    return RedirectToAction("GetUsuarios");
  }

  public IActionResult EliminarUsuario(int id) {
    int? rol = HttpContext.Session.GetInt32("Rol");
    if (rol != (int) RolUsuario.ADMINISTRADOR) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No tienes permiso para realizar esta acción");
      return RedirectToAction("GetUsuarios");
    }

    Usuario usuarioAModificar = _usuarioRepository.GetUsuario(id);
    if (usuarioAModificar == null) {
      AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.ERROR, "No se encontró usuario a eliminar");
      return RedirectToAction("GetUsuarios");
    }

    _usuarioRepository.EliminarUsuario(id);
    AgregarGenericoEstadoATempData(ESTATUS_SEVERIDAD.SUCCESS, "Usuario eliminado con éxito");

    return RedirectToAction("GetUsuarios");
  }

  [HttpGet("login")]
  public IActionResult Login() {
    LoginViewModel loginVM = new LoginViewModel();
    return View(loginVM);
  }

  [HttpPost("login")]
  public IActionResult Login(LoginViewModel loginViewModel) {
    try {
      if (!ModelState.IsValid) {
        loginViewModel.TieneError = true;
        loginViewModel.ErrorMensaje = "Datos invalidos, por favor reintente";
        return View(loginViewModel);
      }

      Usuario usuarioLogueado = _usuarioRepository.Login(new Usuario(loginViewModel));

      if (usuarioLogueado.NombreDeUsario == null) {
        throw new Exception("No se puede loguear usuario");
      }

      HttpContext.Session.SetInt32("UsuarioId", usuarioLogueado.Id);
      HttpContext.Session.SetString("NombreDeUsuario", usuarioLogueado.NombreDeUsario);
      HttpContext.Session.SetInt32("Rol", (int) usuarioLogueado.Rol);

      return RedirectToAction("Index", "Home");
    } catch (Exception e) {
      loginViewModel.TieneError = true;
      loginViewModel.ErrorMensaje = e.Message;
      return View(loginViewModel);
    }
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
