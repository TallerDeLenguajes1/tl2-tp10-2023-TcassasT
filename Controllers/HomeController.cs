using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_TcassasT.Interfaces;
using tl2_tp10_2023_TcassasT.Models;
using tl2_tp10_2023_TcassasT.tl2_tp10_2023_TcassasT.Models;
using tl2_tp10_2023_TcassasT.Utility;
using tl2_tp10_2023_TcassasT.ViewModels;

namespace tl2_tp10_2023_TcassasT.Controllers;

public class HomeController : Controller {
  private readonly ILogger<HomeController> _logger;
  private readonly IUsuarioTableroRepository _usuarioTableroRepository;
  private readonly ITableroReposiroty _tableroReposiroty;
  private readonly IActividadRepository _actividadRepository;
  private readonly IUsuarioRepository _usuarioRepository;

  public HomeController(ILogger<HomeController> logger, IUsuarioTableroRepository usuarioTableroRepository, ITableroReposiroty tableroReposiroty, IActividadRepository actividadRepository, IUsuarioRepository usuarioRepository) {
    _logger = logger;
    _usuarioTableroRepository = usuarioTableroRepository;
    _tableroReposiroty = tableroReposiroty;
    _actividadRepository = actividadRepository;
    _usuarioRepository = usuarioRepository;
  }

  public IActionResult Index() {
    int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioId == null) {
      throw new Exception("Usuario no está logueado");
    }

    Usuario usuario = _usuarioRepository.GetUsuario((int) usuarioId);
    List<int> tablerosPertenecientes = new List<int>();
    List<UsuarioTablero> membresias = _usuarioTableroRepository.GetMembresias((int) usuarioId);
    membresias.ForEach((UsuarioTablero membresia) => {
      tablerosPertenecientes.Add(membresia.TableroId);
    });
    List<TableroExtendido> tableros = _tableroReposiroty.GetTablerosExtendidosByTableroId(tablerosPertenecientes);

    HomeIndexViewModel homeIndexViewModel = new HomeIndexViewModel() {
      tableros = tableros,
      UsuarioNombre = usuario.NombreDeUsario,
      EsAdministrador = (int) usuario.Rol == (int) RolUsuario.ADMINISTRADOR
    };

    if (TempData.Get<EstatsuGenericoViewModel>("Estatus") != null) {
      homeIndexViewModel.Estatus = TempData.Get<EstatsuGenericoViewModel>("Estatus");
    }

    return View(homeIndexViewModel);
  }

  public IActionResult Privacy() {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error() {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
