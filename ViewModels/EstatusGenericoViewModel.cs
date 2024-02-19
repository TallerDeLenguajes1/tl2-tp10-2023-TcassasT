namespace tl2_tp10_2023_TcassasT.ViewModels;

public enum ESTATUS_SEVERIDAD {
  ERROR,
  WARNING,
  SUCCESS
}

public class EstatsuGenericoViewModel {
  public ESTATUS_SEVERIDAD Severidad { get; set; }
  public string EstatusMensaje { get; set; }
  public bool TieneEstatus { get; set; }
}
