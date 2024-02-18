using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class CrearUsuarioViewModel : TienErrorViewModel {
  [Required(ErrorMessage = "Nombre de usuario es requerido")]
  public string NombreDeUsuario { get; set; }
  [Required(ErrorMessage = "Contraseña es requerida")]
  [MinLength(6, ErrorMessage = "Contraseña debe tener por lo menos 6 caracteres")]
  public string Contrasenia { get; set; }
  [Required(ErrorMessage = "Rol es requerido")]
  public RolUsuario Rol { get; set; } = RolUsuario.OPERADOR;
  public bool EsAdministrador { get; set; } = false;
}
