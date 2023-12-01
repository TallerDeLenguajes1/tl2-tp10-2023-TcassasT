using tl2_tp10_2023_TcassasT.Models;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_TcassasT.Interfaces;

namespace tl2_tp10_2023_TcassasT.Controllers;

public class UsuarioController : Controller {
  private readonly ILogger<UsuarioController> _logger;
  private readonly IUsuarioRepository usuarioRepository;
  public UsuarioController(ILogger<UsuarioController> logger) {
      _logger = logger;
      usuarioRepository = new UsuarioRepository();
  }

  [HttpGet]
  public IActionResult GetUsuarios() {
    List<Usuario> usuarios = usuarioRepository.GetUsuarios();
    return View(usuarios);
  }

  [HttpGet]
  public IActionResult CrearUsuario() {
    return View(new Usuario());
  }

  [HttpPost]
  public IActionResult CrearUsuario(Usuario usuario) {
    usuarioRepository.CrearUsuario(usuario);
    return RedirectToAction("GetUsuarios");
  }

  [HttpGet]
  public IActionResult ModificarUsuario(int id) {
    Usuario usuario = usuarioRepository.GetUsuario(id);
    return View(usuario);
  }

  [HttpPost]
  public IActionResult ModificarUsuario(int id, Usuario usuario) {
    usuarioRepository.ModificarUsuario(id, usuario);
    return RedirectToAction("GetUsuarios");
  }

  public ActionResult EliminarUsuario(int id) {
    usuarioRepository.EliminarUsuario(id);
    return RedirectToAction("GetUsuarios");
  }
}