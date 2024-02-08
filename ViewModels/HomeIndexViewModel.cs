using tl2_tp10_2023_TcassasT.Models;

public class HomeIndexViewModel {
  public List<Tablero> tableros = new List<Tablero>();
  public HomeIndexViewModel() {}
  public HomeIndexViewModel(List<Tablero> tableros) {
    this.tableros = tableros;
  }
}