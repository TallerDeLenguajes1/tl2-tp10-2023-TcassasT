using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class CrearTareaViewModel : TienErrorViewModel {
  [Required(ErrorMessage = "Nombre de tarea es requerido")]
  public string Nombre { get; set; }
  [AllowNull]
  [MaxLength(100, ErrorMessage = "Largo máximo de descripción es de 100 caracteres")]
  public string? Descripcion { get; set; }
  [AllowNull]
  public string? Color { get; set; }
  [Required(ErrorMessage = "Estado inicial de tarea es requerido requerido")]
  public EstadoTarea Estado { get; set; }
  [Required(ErrorMessage = "Identificador de usuario asignado es requerido")]
  public int? IdUsuarioAsignado { get; set; }
  [Required(ErrorMessage = "Identificador de tablero es requerido")]
  public int IdTablero { get; set; }
}
