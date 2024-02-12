using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class GetTareasYTableroViewModel {
  public Tablero Tablero { get; set; } = new Tablero();
  public List<Tarea> Tareas { get; set; } = new List<Tarea>();
}
