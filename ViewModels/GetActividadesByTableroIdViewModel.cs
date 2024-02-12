using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class GetActividadesByTableroIdViewModel {
  public Tablero Tablero { get; set; } = new Tablero();
  public List<ActividadExtendida> Actividades { get; set; } = new List<ActividadExtendida>();
}
