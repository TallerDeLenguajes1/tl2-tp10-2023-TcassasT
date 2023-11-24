namespace tl2_tp10_2023_TcassasT.Models;

public enum EstadoTarea {
  IDEAS,
  TODO,
  DOING,
  REVIEW,
  DONE
}

public class Tarea {
  private int id;
  private String? nombre;
  private String? descripcion;
  private String? color;
  private EstadoTarea estado;
  private int? idUsuarioAsignado;
  private int idTablero;

  public int Id { get => id; set => id = value; }
  public String? Nombre { get => nombre; set => nombre = value; }
  public String? Descripcion { get => descripcion; set => descripcion = value; }
  public String? Color { get => color; set => color = value; }
  public EstadoTarea Estado { get => estado; set => estado = value; }
  public int? IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }
  public int IdTablero { get => idTablero; set => idTablero = value; }
}