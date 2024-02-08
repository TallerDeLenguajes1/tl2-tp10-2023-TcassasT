namespace tl2_tp10_2023_TcassasT.Interfaces;

public interface IUsuarioTableroRepository {
  void AgregarUsuarioATablero(int usuarioId, int tableroId);
  void RemoverUsuarioDeTablero(int usuarioId, int tableroId);
  List<UsuarioTablero> GetMembresias(int usuarioId);
}