namespace tl2_tp10_2023_TcassasT.Models;

public enum RolUsuario {
  ADMINISTRADOR,
  OPERADOR,
}

public class Usuario {
  private int id;
  private String? nombreDeUsuario;
  private RolUsuario rol;
  private String? contrasenia;

  public int Id { get => id; set => id = value; }
  public String? NombreDeUsario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
  public RolUsuario Rol { get => rol; set => rol = value; }
  public String? Contrasenia { get => contrasenia; set => contrasenia = value; }
}