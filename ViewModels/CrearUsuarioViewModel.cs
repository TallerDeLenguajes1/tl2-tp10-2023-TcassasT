using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.ViewModels;

public class CrearUsuarioViewModel : TienErrorViewModel {
  public Usuario Usuario { get; set; } = new Usuario() {
    Rol = RolUsuario.OPERADOR
  };
  public bool EsAdministrador { get; set; } = false;
}
