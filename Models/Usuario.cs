using tl2_tp10_2023_TcassasT.ViewModels;

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

  public Usuario() {}
  public Usuario(CrearUsuarioViewModel usuarioViewModel) {
    this.nombreDeUsuario = usuarioViewModel.NombreDeUsuario;
    this.contrasenia = usuarioViewModel.Contrasenia;
    this.rol = usuarioViewModel.Rol;
  }

  public Usuario(LoginViewModel loginViewModel) {
    this.nombreDeUsuario = loginViewModel.NombreDeUsuario;
    this.contrasenia = loginViewModel.Contrasenia;
  }
}
