using tl2_tp10_2023_TcassasT.Models;

public class HomeIndexViewModel {
  public List<TableroExtendido> tableros = new List<TableroExtendido>();
  public HomeIndexViewModel() {}
  public HomeIndexViewModel(List<TableroExtendido> tableros) {
    this.tableros = tableros;
  }
}
