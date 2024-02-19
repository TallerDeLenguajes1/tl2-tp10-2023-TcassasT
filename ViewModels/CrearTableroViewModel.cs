using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class CrearTableroViewModel : TienErrorViewModel {
  [Required(ErrorMessage = "No existe referencia de propietario para crear tablero")]
  public int IdUsuarioPropietario { get; set; }
  [Required(ErrorMessage = "Nombre del tablero es un campo requerido")]
  [MaxLength(40, ErrorMessage = "El nombre no puede tener mas de 40 caracteres")]
  public string Nombre { get; set; }
  [AllowNull]
  [MaxLength(100, ErrorMessage = "Largo máximo de descripción es de 100 caracteres")]
  public string? Descripcion { get; set; }
}
