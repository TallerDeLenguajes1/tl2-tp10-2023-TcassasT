using tl2_tp10_2023_TcassasT.ViewModels;

namespace tl2_tp10_2023_TcassasT.Models;

public enum EstadoTarea {
  IDEAS,
  TODO,
  DOING,
  REVIEW,
  DONE
}

public enum ArchivadoTarea {
  NO_ARCHIVADO,
  ARCHIVADO
}

public class Tarea {
  private int id;
  private String? nombre;
  private String? descripcion;
  private String? color;
  private EstadoTarea estado;
  private int? idUsuarioAsignado;
  private int idTablero;
  private ArchivadoTarea archivada;

  public int Id { get => id; set => id = value; }
  public String? Nombre { get => nombre; set => nombre = value; }
  public String? Descripcion { get => descripcion; set => descripcion = value; }
  public String? Color { get => color; set => color = value; }
  public EstadoTarea Estado { get => estado; set => estado = value; }
  public int? IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }
  public int IdTablero { get => idTablero; set => idTablero = value; }
  public ArchivadoTarea Archivada { get => archivada; set => archivada = value; }

  public Tarea() {}
  public Tarea(CrearTareaViewModel crearTareaVM) {
    this.Nombre = crearTareaVM.Nombre;
    this.Descripcion = crearTareaVM.Descripcion;
    this.Color = crearTareaVM.Color;
    this.Estado = crearTareaVM.Estado;
    this.IdUsuarioAsignado = crearTareaVM.IdUsuarioAsignado;
    this.IdTablero = crearTareaVM.IdTablero;
  }

  public Tarea(ModificarTareaViewModel modificarTareaVM) {
    this.Id = modificarTareaVM.Id;
    this.Nombre = modificarTareaVM.Nombre;
    this.Descripcion = modificarTareaVM.Descripcion;
    this.Color = modificarTareaVM.Color;
    this.Estado = modificarTareaVM.Estado;
    this.IdUsuarioAsignado = modificarTareaVM.IdUsuarioAsignado;
    this.IdTablero = modificarTareaVM.IdTablero;
  }
}

public class TareaArchivada : Tarea {
  private string? archivadaFecha;
  public string? ArchivadaFecha { get => archivadaFecha; set => archivadaFecha = value; }
  public string? ArchivadaPorUsuario { get; set; }
}
