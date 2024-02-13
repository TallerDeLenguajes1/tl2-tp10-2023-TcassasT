using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class GetMiembrosByTableroIdViewModel {
  public int UsuarioLogueado;
  public TableroMembrecias TableroMembrecias = new TableroMembrecias();
  public Boolean PuedeAdministrarMiembros = false;
}
