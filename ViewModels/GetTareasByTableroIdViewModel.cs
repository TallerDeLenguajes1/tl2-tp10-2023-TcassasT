using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class GetTareasByTableroIdViewModel {
  public int TableroId { get; set; }
  public int CantidadDeTareas { get; set; }
  public Dictionary<EstadoTarea, List<Tarea>> TareasPorEstado { get; set; }

  public GetTareasByTableroIdViewModel() {
    TareasPorEstado = Enum.GetValues(typeof(EstadoTarea))
      .Cast<EstadoTarea>()
      .ToDictionary(tipo => tipo, tipo => new List<Tarea>());
  }
}