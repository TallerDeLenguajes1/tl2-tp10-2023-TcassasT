using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class ModificarTableroViewModel : CrearTableroViewModel {
  public int Id { get; set; }

  public ModificarTableroViewModel() {}
  public ModificarTableroViewModel(Tablero tablero) {
    this.Id = tablero.Id;
    this.IdUsuarioPropietario = tablero.IdUsuarioPropietario;
    this.Nombre = tablero.Nombre;
    this.Descripcion = tablero.Descripcion;
  }
}
