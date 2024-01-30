using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class GetTareasByTableroIdViewModel {
  public int TableroId { get; set; }
  public List<Tarea> Tareas { get; set; } = new List<Tarea>();

  public GetTareasByTableroIdViewModel() {}
  public GetTareasByTableroIdViewModel(int tableroId, List<Tarea> tareas) {
    this.TableroId = tableroId;
    this.Tareas = tareas;
  }
}
