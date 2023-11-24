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
}