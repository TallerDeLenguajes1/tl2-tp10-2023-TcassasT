using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class GetUsuariosViewModel {
  public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
  public EstatsuGenericoViewModel Estatus = new EstatsuGenericoViewModel();
}
