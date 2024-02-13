namespace tl2_tp10_2023_TcassasT.Models;

public class Tablero {
  private int id;
  private int idUsuarioPropietario;
  private String? nombre;
  private String? descripcion;

  public int Id { get => id; set => id = value; }
  public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
  public String? Nombre { get => nombre; set => nombre = value; }
  public String? Descripcion { get => descripcion; set => descripcion = value; }
}

public class TableroExtendido : Tablero {
  private List<ActividadExtendida> actividades = new List<ActividadExtendida>();
  public List<ActividadExtendida> Actividades { get => actividades; set => actividades = value; }
}

public class TableroMembrecias : Tablero {
  private List<Usuario> miembros = new List<Usuario>();
  public List<Usuario> Miembros { get => miembros; set => miembros = value; }
}
