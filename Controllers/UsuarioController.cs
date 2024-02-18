using tl2_tp10_2023_TcassasT.Models;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_TcassasT.Interfaces;
using tl2_tp10_2023_TcassasT.ViewModels;

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
    List<Usuario> usuarios = _usuarioRepository.GetUsuarios();
    return View(usuarios);
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
    Usuario usuario = _usuarioRepository.GetUsuario(id);
    return View(usuario);
  }

  [HttpPost("modificar/{id}")]
  public IActionResult ModificarUsuario(int id, Usuario usuario) {
    _usuarioRepository.ModificarUsuario(id, usuario);
    return RedirectToAction("GetUsuarios");
  }

  public IActionResult EliminarUsuario(int id) {
    _usuarioRepository.EliminarUsuario(id);
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
}