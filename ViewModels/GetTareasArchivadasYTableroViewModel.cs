using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class GetTareasArchivadasYTableroViewModel {
  public Tablero Tablero { get; set; } = new Tablero();
  public List<TareaArchivada> Tareas { get; set; } = new List<TareaArchivada>();
}
