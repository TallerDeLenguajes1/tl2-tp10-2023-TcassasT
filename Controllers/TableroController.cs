using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_TcassasT.Interfaces;
using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.Controllers;

public class TableroController: Controller {
  private readonly ILogger<TableroController> _logger;
  private readonly ITableroReposiroty tableroReposiroty;
  public TableroController(ILogger<TableroController> logger) {
      _logger = logger;
      tableroReposiroty = new TableroRepository();
  }

  [HttpGet]
  public IActionResult GetTableros() {
    int userId = 1;
    List<Tablero> tableros = tableroReposiroty.GetTablerosByUserId(userId);
    return View(tableros);
  }

  [HttpGet]
  public IActionResult CrearTablero() {
    return View(new Tablero());
  }

  [HttpPost]
  public IActionResult CrearTablero(Tablero tablero) {
    tableroReposiroty.CrearTablero(tablero);
    return RedirectToAction("GetTableros");
  }

  [HttpGet]
  public IActionResult ModificarTablero(int id) {
    Tablero tablero = tableroReposiroty.GetTablero(id);
    return View(tablero);
  }

  [HttpPost]
  public IActionResult ModificarTablero(int id, Tablero tablero) {
    tableroReposiroty.ModificarTablero(id, tablero);
    return RedirectToAction("GetTableros");
  }

  public IActionResult EliminarTablero(int id) {
    tableroReposiroty.EliminarTablero(id);
    return RedirectToAction("GetTableros");
  }
}