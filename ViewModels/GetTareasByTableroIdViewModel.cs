using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class GetTareasByTableroIdViewModel {
  public Tablero Tablero { get; set; }
  public int CantidadDeTareas { get; set; }
  public Dictionary<EstadoTarea, List<Tarea>> TareasPorEstado { get; set; }

  public GetTareasByTableroIdViewModel() {
    Tablero = new Tablero();
    TareasPorEstado = Enum.GetValues(typeof(EstadoTarea))
      .Cast<EstadoTarea>()
      .ToDictionary(tipo => tipo, tipo => new List<Tarea>());
  }
}
