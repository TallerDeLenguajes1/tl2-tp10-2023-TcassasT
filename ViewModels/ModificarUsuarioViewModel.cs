using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class ModificarUsuarioViewModel : TienErrorViewModel {
  [Required(ErrorMessage = "Nombre de usuario es requerido")]
  public string NombreDeUsuario { get; set; }
  [Required(ErrorMessage = "Identificador de usuario es requerido")]
  public int Id { get; set; }
  [Required(ErrorMessage = "Rol es requerido")]
  public RolUsuario Rol { get; set; } = RolUsuario.OPERADOR;

  public ModificarUsuarioViewModel() {}
  public ModificarUsuarioViewModel(Usuario usuario) {
    this.Id = usuario.Id;
    this.NombreDeUsuario = usuario.NombreDeUsario;
    this.Rol = usuario.Rol;
  }
}
