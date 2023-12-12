using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.Interfaces;

public interface IUsuarioRepository {
  void CrearUsuario(Usuario usuario);
  void ModificarUsuario(int id, Usuario usuario);
  List<Usuario> GetUsuarios();
  Usuario GetUsuario(int id);
  void EliminarUsuario(int id);
  Usuario Login(Usuario usuario);
}