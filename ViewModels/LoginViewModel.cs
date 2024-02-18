using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class LoginViewModel : TienErrorViewModel {
  [Required(ErrorMessage = "Nombre usuario es requerido")]
  public string NombreDeUsuario { get; set; }
  [Required(ErrorMessage = "Contraseña es requerido")]
  public string Contrasenia { get; set; }
}
