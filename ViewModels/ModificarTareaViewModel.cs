using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class ModificarTareaViewModel : CrearTareaViewModel {
  [Required(ErrorMessage = "Identificador de tarea es requerido")]
  public int Id { get; set; }

  public ModificarTareaViewModel() {}
  public ModificarTareaViewModel(Tarea tarea) {
    this.Id = tarea.Id;
    this.Nombre = tarea.Nombre;
    this.Descripcion = tarea.Descripcion;
    this.Color = tarea.Color;
    this.Estado = tarea.Estado;
    this.IdTablero = tarea.IdTablero;
    this.IdUsuarioAsignado = tarea.IdUsuarioAsignado;
  }
}
