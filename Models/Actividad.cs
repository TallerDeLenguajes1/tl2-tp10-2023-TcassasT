namespace tl2_tp10_2023_TcassasT.Models;

public class Actividad {
  private int id;
  private int usuarioId;
  private int tableroId;
  private int tareaId;
  private string? actividadTexto;
  private DateTime fecha;
  public int Id { get => id; set => id = value; }
  public int UsuarioId { get => usuarioId; set => usuarioId = value; }
  public int TableroId { get => tableroId; set => tableroId = value; }
  public int TareaId { get => tareaId; set => tareaId = value; }
  public string? ActividadTexto { get => actividadTexto; set => actividadTexto = value; }
  public DateTime Fecha { get => fecha; set => fecha = value; }
}
