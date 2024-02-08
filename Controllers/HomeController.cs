using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_TcassasT.Interfaces;
using tl2_tp10_2023_TcassasT.Models;
using tl2_tp10_2023_TcassasT.tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUsuarioTableroRepository _usuarioTableroRepository;
    private readonly ITableroReposiroty _tableroReposiroty;

    public HomeController(ILogger<HomeController> logger, IUsuarioTableroRepository usuarioTableroRepository, ITableroReposiroty tableroReposiroty) {
        _logger = logger;
        _usuarioTableroRepository = usuarioTableroRepository;
        _tableroReposiroty = tableroReposiroty;
    }

    public IActionResult Index() {
        int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

        if (usuarioId == null) {
            throw new Exception("Usuario no está logueado");
        }

        List<int> tablerosPertenecientes = new List<int>();
        List<UsuarioTablero> membresias = _usuarioTableroRepository.GetMembresias((int) usuarioId);
        membresias.ForEach((UsuarioTablero membresia) => {
            tablerosPertenecientes.Add(membresia.TableroId);
        });
        List<Tablero> tableros = _tableroReposiroty.GetTablerosByTableroId(tablerosPertenecientes);

        HomeIndexViewModel homeIndexViewModel = new HomeIndexViewModel() { tableros = tableros };

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
