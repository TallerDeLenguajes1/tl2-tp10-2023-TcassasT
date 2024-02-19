using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class GetTablerosViewModel : EstatsuGenericoViewModel {
  public List<TableroMembrecias> Tableros { get; set; }
  public EstatsuGenericoViewModel? Estatus { get; set; }
}
