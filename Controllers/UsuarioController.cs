using tl2_tp10_2023_TcassasT.Models;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_TcassasT.Interfaces;

namespace tl2_tp10_2023_TcassasT.Controllers;

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

  [HttpGet]
  public IActionResult CrearUsuario() {
    return View(new Usuario());
  }

  [HttpPost]
  public IActionResult CrearUsuario(Usuario usuario) {
    _usuarioRepository.CrearUsuario(usuario);
    return RedirectToAction("GetUsuarios");
  }

  [HttpGet]
  public IActionResult ModificarUsuario(int id) {
    Usuario usuario = _usuarioRepository.GetUsuario(id);
    return View(usuario);
  }

  [HttpPost]
  public IActionResult ModificarUsuario(int id, Usuario usuario) {
    _usuarioRepository.ModificarUsuario(id, usuario);
    return RedirectToAction("GetUsuarios");
  }

  public IActionResult EliminarUsuario(int id) {
    _usuarioRepository.EliminarUsuario(id);
    return RedirectToAction("GetUsuarios");
  }

  [HttpGet]
  public IActionResult Login() {
    return View(new Usuario());
  }

  [HttpPost]
  public IActionResult Login(Usuario usuario) {
    Usuario usuarioLogueado = _usuarioRepository.Login(usuario);
    if (usuarioLogueado.NombreDeUsario == null) {
      throw new Exception("No se puede loguear usuario");
    }

    HttpContext.Session.SetInt32("UsuarioId", usuarioLogueado.Id);
    HttpContext.Session.SetString("NombreDeUsuario", usuarioLogueado.NombreDeUsario);
    HttpContext.Session.SetString("Rol", Convert.ToString((int) usuarioLogueado.Rol));

    return RedirectToAction("Index", "Home");
  }
}