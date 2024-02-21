using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.Interfaces;

public interface IUsuarioRepository {
  void CrearUsuario(Usuario usuario);
  void ModificarUsuario(int id, Usuario usuario);
  List<Usuario> GetUsuarios();
  List<Usuario> GetMiembrosDeTablero(int idTablero);
  List<Usuario> GetCandidatosAMiembrosDeTablero(int idTablero, string busqueda);
  Usuario? GetUsuario(int id);
  Usuario? GetUsuario(string nombreDeUsuario);
  void EliminarUsuario(int id);
  Usuario Login(Usuario usuario);
  bool UsuarioEsAdministrador(int id);
}