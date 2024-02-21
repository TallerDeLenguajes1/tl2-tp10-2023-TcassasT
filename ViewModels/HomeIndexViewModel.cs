using tl2_tp10_2023_TcassasT.Models;
using tl2_tp10_2023_TcassasT.ViewModels;

public class HomeIndexViewModel {
  public List<TableroExtendido> tableros { get; set; } = new List<TableroExtendido>();
  public string UsuarioNombre { get; set; }
  public bool EsAdministrador { get; set; }
  public EstatsuGenericoViewModel Estatus { get; set; }
  public HomeIndexViewModel() {}
  public HomeIndexViewModel(List<TableroExtendido> tableros, string usuarioNombre, bool esAdministrador, EstatsuGenericoViewModel estatus) {
    this.tableros = tableros;
    this.UsuarioNombre = usuarioNombre;
    this.EsAdministrador = esAdministrador;
    this.Estatus = estatus;
  }
}
